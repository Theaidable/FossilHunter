using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Chat_Test
{
    /// <summary>
    /// Basal TCP-server til en chatfunktion
    /// </summary>
    class TCPServer
    {
        //Fields
        private TcpListener _listener;
        private ConcurrentDictionary<TcpClient, (StreamReader _reader, StreamWriter _writer, string name)> _clients = new ConcurrentDictionary<TcpClient, (StreamReader _reader, StreamWriter _writer, string name)>();

        /// <summary>
        /// Constructor for serveren
        /// </summary>
        /// <param name="ip"></param>
        public TCPServer(IPAddress ip, int port)
        {
            _listener = new TcpListener(ip, port);
        }

        static void Main(string[] args)
        {
            // Lyt på alle netkort (IPv4/IPv6) på port 12000 – undgå <1024, de kræver admin.
            // Slide om porte/sockets pointer: brug ikke "well-known" porte < 1024 til egne apps.
            int port = args.Length > 0 && int.TryParse(args[0], out var p) ? p : 12000;
            var server = new TCPServer(IPAddress.Any, port);
            server.Start();
        }

        public void Start()
        {
            _listener.Start();
            Console.WriteLine($"[SERVER] Lytter på {_listener.LocalEndpoint} (CTRL+C for at stoppe)");

            while (true)
            {
                var client = _listener.AcceptTcpClient();

                //Opret tråd til at håndtere client
                var thread = new Thread(() => HandleClient(client))
                {
                    //Sørg for at tråd køres i baggrunden
                    IsBackground = true
                };

                //Start tråd
                thread.Start();
            }
        }

        private void HandleClient(TcpClient client)
        {
            var stream = client.GetStream();

            var reader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);
            var writer = new StreamWriter(stream, Encoding.UTF8, bufferSize: 1024, leaveOpen: true)
            {
                AutoFlush = true // Så vi slipper for at kalde Flush() efter hver linje
            };

            writer.WriteLine("Welcom, please enter your display name:");
            string? name = reader.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                writer.WriteLine("ERROR: Name required. Disconnecting.");
                client.Close();
                return;
            }

            _clients[client] = (reader, writer, name);
            Console.WriteLine($"[SERVER] {name} tilsluttet fra {client.Client.RemoteEndPoint}");

            Broadcast($"*** {name} joined chat ***", from: null);

            try
            {
                string? line;
                while (client.Connected && (line = reader.ReadLine()) != null)
                {
                    // Simple command: /quit
                    if (line.Equals("/quit", StringComparison.OrdinalIgnoreCase))
                    {
                        writer.WriteLine("BYE");
                        break;
                    }

                    // Broadcast til alle andre
                    Broadcast($"{name}: {line}", from: client);
                }
            }
            catch (IOException)
            {
                // Netværksfejl/forbindelse lukket – forventeligt i praksis
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SERVER] Fejl for {name}: {ex.Message}");
            }
            finally
            {
                // Ryd op
                _clients.TryRemove(client, out _);
                try
                {
                    client.Close();
                }

                catch
                {
                    /* ignore */
                }

                Console.WriteLine($"[SERVER] {name} forlod chatten");
                Broadcast($"*** {name} left chat ***", from: null);
            }
        }

        private void Broadcast(string message, TcpClient? from)
        {
            foreach (var kv in _clients)
            {
                var client = kv.Key;
                var (_, writer, _) = kv.Value;

                // Hvis from == client, så spring over (eco undgås)
                if (from != null && client == from) continue;

                try
                {
                    writer.WriteLine(message);
                }
                catch
                {
                    // Hvis skrivning fejler, fjerner vi klienten næste gang loopet rammer den
                }
            }
        }
    }
}