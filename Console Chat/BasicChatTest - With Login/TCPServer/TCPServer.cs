using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Chat_Test
{
    /// <summary>
    /// Selve TCP Serveren som klienterne forbinder til
    /// Håndtere Login/Register
    /// Modtager chat-beskeder og broadcaster dem
    /// </summary>
    class TCPServer
    {
        // Lytter på en given IP/port efter indkommende forbindelser.
        private readonly TcpListener _listener;

        // Holder styr på forbundne klienter:
        // - Reader/Writer for at læse/skrives linjer
        // - "name" = brugernavn når man er logget ind
        private readonly ConcurrentDictionary<TcpClient, (StreamReader r, StreamWriter w, string? name)> _clients = new ConcurrentDictionary<TcpClient, (StreamReader r, StreamWriter w, string? name)>();

        // Simpel "brugerdatabase" i hukommelsen (hash + salt for passwords).
        private readonly UserStore _users = new();

        /// <summary>
        /// Constructor for serveren
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public TCPServer(IPAddress ip, int port)
        {
            _listener = new TcpListener(ip, port);
        }

        public static void Main(string[] args)
        {
            // Brug en høj port (>=1024). Kan sættes via argument, ellers 12000.
            int port = (args.Length > 0 && int.TryParse(args[0], out var p)) ? p : 12000;

            var server = new TCPServer(IPAddress.Any, port);

            server.Start();
        }

        public void Start()
        {
            _listener.Start();

            Console.WriteLine($"Server kører på port {((_listener.LocalEndpoint as IPEndPoint)?.Port ?? 0)}");

            // Acceptér forbindelser i en simpel løkke.
            while (true)
            {
                var client = _listener.AcceptTcpClient();

                // Start en baggrundstråd pr. klient.
                var thread = new Thread(() => HandleClient(client)) 
                { 
                    IsBackground = true 
                };

                thread.Start();
            }
        }

        private async void HandleClient(TcpClient client)
        {
            // Sæt tekstlæsning/skrivning op (UTF-8, linje-baseret).
            var stream = client.GetStream();
            var reader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);
            var writer = new StreamWriter(stream, new UTF8Encoding(false), 1024, leaveOpen: true) 
            { 
                AutoFlush = true 
            };

            // Registrér klienten.
            _clients[client] = (reader, writer, null);
            string? name = null;

            try
            {
                // Læs én linje ad gangen (hver linje er JSON).
                while (true)
                {
                    var line = await reader.ReadLineAsync();
                    if (line == null)
                    {
                        break; // klienten lukkede forbindelsen
                    }

                    ClientEnvelope? msg = null;

                    try 
                    { 
                        msg = JsonSerializer.Deserialize<ClientEnvelope>(line); 
                    }
                    catch 
                    { 
                        await Send(writer, new ServerMsg("error", "invalid_json")); 
                        continue; 
                    }

                    // "type" afgør hvilken handling vi udfører.
                    switch (msg!.Type?.ToLowerInvariant())
                    {
                        case "register":

                            // Tjek input
                            if (string.IsNullOrWhiteSpace(msg.Username) || string.IsNullOrWhiteSpace(msg.Password))
                            { 
                                await Send(writer, new ServerMsg("error", "missing_fields")); 
                                break; 
                            }

                            // Forsøg at oprette (fejler hvis brugernavn findes).
                            if (!_users.TryAddUser(msg.Username!, msg.Password!))
                            {
                                await Send(writer, new ServerMsg("error", "username_taken"));
                            }
                            else
                            {
                                await Send(writer, new ServerMsg("ok", "registered"));
                            }
                            break;

                        case "login":

                            // Tjek input
                            if (string.IsNullOrWhiteSpace(msg.Username) || string.IsNullOrWhiteSpace(msg.Password))
                            { 
                                await Send(writer, new ServerMsg("error", "missing_fields")); 
                                break; 
                            }

                            // Valider credentials (hash+salt sammenlignes).
                            if (_users.Validate(msg.Username!, msg.Password!))
                            {
                                name = msg.Username!;
                                _clients[client] = (reader, writer, name);
                                await Send(writer, new ServerMsg("ok", "logged_in"));
                            }
                            else
                            {
                                await Send(writer, new ServerMsg("error", "bad_credentials"));
                            }
                            break;

                        case "chat":

                            // Kun loggede brugere må sende chat.
                            if (name == null) 
                            { 
                                await Send(writer, new ServerMsg("error", "not_logged_in")); 
                                break; 
                            }

                            var text = msg.Text ?? "";
                            var chat = new ChatMsg("msg", name, text, DateTime.UtcNow.ToString("O"));

                            // Send til alle udover afsender.
                            await Broadcast(chat, except: client);
                            break;

                        case "quit":

                            // klienten lukker selv bagefter.
                            await Send(writer, new ServerMsg("bye", "goodbye"));
                            return;

                        default:
                            await Send(writer, new ServerMsg("error", "unknown_type"));
                            break;
                    }
                }
            }
            catch
            {
                // Netværksfejl/forbindelse afbrudt – helt normalt at ignorere i prototype.
            }
            finally
            {
                // Fjern klient og luk socket.
                _clients.TryRemove(client, out _);
                try 
                { 
                    client.Close(); 
                } 
                catch { }
            }
        }

        // Svarer én klient med et JSON-objekt (på sin egen linje).
        private static async Task Send(StreamWriter w, object payload)
        {
            await w.WriteLineAsync(JsonSerializer.Serialize(payload));
        }

        // Sender et JSON-objekt til alle klienter (undtagen "except").
        private async Task Broadcast(object payload, TcpClient? except = null)
        {
            var json = JsonSerializer.Serialize(payload) + "\n";
            var data = Encoding.UTF8.GetBytes(json);

            foreach (var kv in _clients)
            {
                if (except != null && kv.Key == except)
                {
                    continue; // hop over afsender
                }

                try 
                { 
                    await kv.Key.GetStream().WriteAsync(data); 
                }
                catch { }
            }
        }
    }
}
