using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Chat_Test
{
    /// <summary>
    /// Client til at prøve at skrive til server
    /// </summary>
    class TCPClient
    {
        static void Main(string[] args)
        {
            string host = args.Length > 0 ? args[0] : "localhost"; // 127.0.0.1 er loopback
            int port = args.Length > 1 && int.TryParse(args[1], out var p) ? p : 12000;

            using var client = new TcpClient();
            client.Connect(host, port);
            Console.WriteLine($"[CLIENT] Forbundet til {host}:{port}");

            var stream = client.GetStream();
            using var reader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);
            using var writer = new StreamWriter(stream, Encoding.UTF8, bufferSize: 1024, leaveOpen: true)
            {
                AutoFlush = true
            };

            // Modtagertråd – læs alt fra serveren og print
            var recieve = new Thread(() =>
            {
                try
                {
                    string? line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                    }
                }
                catch (IOException)
                {
                    // Forbindelsen lukket
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[CLIENT] Fejl i modtagelse: {ex.Message}");
                }
            });

            recieve.IsBackground = true;
            recieve.Start();

            // Navn leveres som første input, fordi serveren beder om det
            while (true)
            {
                string? line = Console.ReadLine();

                if (line == null)
                {
                    break; // Ctrl+Z/Ctrl+D
                }

                writer.WriteLine(line);

                if (string.Equals(line, "/quit", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }
            }

            Console.WriteLine("[CLIENT] Lukker ned...");
        }
    }
}