using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Chat_Test
{
    /// <summary>
    /// Selve clienten som forbinder til serveren
    /// </summary>
    class TCPClient
    {
        public static async Task Main(string[] args)
        {
            // Standard: localhost:12000. Kan angives som argumenter.
            string host = args.Length > 0 ? args[0] : "127.0.0.1";
            int port = (args.Length > 1 && int.TryParse(args[1], out var portValue)) ? portValue : 12000;

            using var client = new TcpClient();
            await client.ConnectAsync(host, port);

            var stream = client.GetStream();
            using var reader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);
            using var writer = new StreamWriter(stream, new UTF8Encoding(false), 1024, leaveOpen: true) 
            { 
                AutoFlush = true 
            };

            // Enkelt auth-flow: vi holder konsollen ren og venlig
            Console.WriteLine("1) Opret bruger   2) Log ind");
            var choice = Console.ReadLine();

            bool loggedIn = false;

            if (choice == "1")
            {
                // Register: bliv ved til det lykkes (brugernavn ledigt + efterfølgende login).
                while (true)
                {
                    var (u, pwd) = PromptCredentials();
                    await Send(writer, new { type = "register", username = u, password = pwd });

                    var resp = await ReadOneAsync(reader);
                    if (resp.type == "ok" && resp.message == "registered")
                    {
                        // Kør login direkte bagefter (med samme credentials),
                        // men hvis login fejler, spørg igen.
                        while (!loggedIn)
                        {
                            await Send(writer, new { type = "login", username = u, password = pwd });
                            var loginResp = await ReadOneAsync(reader);

                            if (loginResp.type == "ok" && loginResp.message == "logged_in")
                            {
                                loggedIn = true;
                            }
                            else
                            {
                                Console.WriteLine("Forkert login. Prøv igen.");
                                (u, pwd) = PromptCredentials();
                            }
                        }
                        break;
                    }
                    else if (resp.type == "error" && resp.message == "username_taken")
                    {
                        Console.WriteLine("Brugernavnet er allerede taget. Vælg et andet.");
                        continue;
                    }
                    else if (resp.type == "error" && resp.message == "missing_fields")
                    {
                        Console.WriteLine("Manglende felter. Prøv igen.");
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("Uventet svar. Prøv igen.");
                    }
                }
            }
            else
            {
                // Login: bliv ved til det lykkes (forkert kode/brugernavn -> prøv igen).
                while (!loggedIn)
                {
                    var (u, pwd) = PromptCredentials();
                    await Send(writer, new { type = "login", username = u, password = pwd });
                    var loginResp = await ReadOneAsync(reader);

                    if (loginResp.type == "ok" && loginResp.message == "logged_in")
                    {
                        loggedIn = true;
                    }
                    else if (loginResp.type == "error" && loginResp.message == "bad_credentials")
                    {
                        Console.WriteLine("Forkert brugernavn eller kodeord. Prøv igen.");
                    }
                    else if (loginResp.type == "error" && loginResp.message == "missing_fields")
                    {
                        Console.WriteLine("Manglende felter. Prøv igen.");
                    }
                    else
                    {
                        Console.WriteLine("Uventet svar. Prøv igen.");
                    }
                }
            }

            // Nu er vi logget ind: start baggrundslæser, der kun viser chatbeskeder
            _ = Task.Run(async () =>
            {
                try
                {
                    string? line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        try
                        {
                            using var doc = JsonDocument.Parse(line);
                            var root = doc.RootElement;
                            var type = root.GetProperty("type").GetString();

                            if (type == "msg")
                            {
                                var from = root.GetProperty("from").GetString();
                                var text = root.GetProperty("text").GetString();
                                Console.WriteLine($"{from}: {text}");
                            }
                            // Alt andet (ok/error/system/bye) ignoreres for at holde UI'et rent.
                        }
                        catch
                        {
                            // Uventet/korrupt linje ignoreres.
                        }
                    }
                }
                catch { /* serveren lukkede, bare afslut */ }
            });

            Console.WriteLine("Skriv beskeder. Brug /quit for at forlade.");
            while (true)
            {
                var text = Console.ReadLine();
                if (text == null) break;

                if (text.Equals("/quit", StringComparison.OrdinalIgnoreCase))
                {
                    await Send(writer, new { type = "quit" });
                    break;
                }

                await Send(writer, new { type = "chat", text });
            }
        }

        // Beder brugeren om brugernavn og skjult kodeord.
        static (string user, string pass) PromptCredentials()
        {
            Console.Write("Brugernavn: ");
            var u = Console.ReadLine() ?? "";
            Console.Write("Kodeord: ");
            var pwd = ReadPassword();
            return (u, pwd);
        }

        // Læser ét svar fra serveren og returnerer kun (type, message).
        // Bruges i auth-fasen, hvor vi kun er interesseret i "ok"/"error".
        static async Task<(string type, string? message)> ReadOneAsync(StreamReader r)
        {
            while (true)
            {
                var line = await r.ReadLineAsync();
                if (line == null) return ("error", "disconnect");

                try
                {
                    using var doc = JsonDocument.Parse(line);
                    var root = doc.RootElement;
                    var t = root.GetProperty("type").GetString() ?? "";
                    string? m = root.TryGetProperty("message", out var mm) ? mm.GetString() : null;

                    if (t is "ok" or "error")
                    {
                        return (t, m);
                    }
                }
                catch { }
            }
        }

        // Sender et JSON-objekt som en linje.
        static async Task Send(StreamWriter w, object obj)
        {
            await w.WriteLineAsync(JsonSerializer.Serialize(obj));
        }

        // Læser kodeord som **** i konsollen (ikke sikkerhed, bare pænt).
        static string ReadPassword()
        {
            var sb = new StringBuilder();
            while (true)
            {
                var k = Console.ReadKey(true);
                if (k.Key == ConsoleKey.Enter) 
                { 
                    Console.WriteLine(); 
                    break; 
                }
                if (k.Key == ConsoleKey.Backspace && sb.Length > 0) 
                { 
                    sb.Length--; Console.Write("\b \b"); 
                }
                else if (!char.IsControl(k.KeyChar)) 
                { 
                    sb.Append(k.KeyChar); 
                    Console.Write("*"); 
                }
            }
            return sb.ToString();
        }
    }
}
