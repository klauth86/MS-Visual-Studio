using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TcpServer
{
    static class ServerHelper
    {
        private static TcpListener TcpListener { get; set; }
        private static Dictionary<TcpClient, GameEngine> Games { get; set; }

        public static void StartServer(string IP = "127.0.0.1", int port = 2275)
        {
            IPAddress address = IPAddress.Parse(IP);

            TcpListener = new TcpListener(address, port);
            TcpListener.Start();

            Games = new Dictionary<TcpClient, GameEngine>();

            Console.WriteLine($"Server started at {IP}:{port}");
        }

        public static void Listen()
        {
            if (TcpListener != null)
            {
                while (true)
                {
                    _listen();
                }
            }
        }

        private async static void _listen()
        {
            var clientTask = TcpListener.AcceptTcpClientAsync();

            if (clientTask.Result != null)
            {
                var client = clientTask.Result;
                await Task.Run(() =>
                {
                    Games.Add(client, new GameEngine());

                    var intLength = 1 + 3 * Games[client].UnitsDict.Count;
                    var bytes = new byte[(intLength + 1) * sizeof(int)];
                    var array = new int[intLength + 1];

                    array[0] = intLength;
                    array[1] = Games[client].Dimension;
                    int i = 2;
                    foreach (var item in Games[client].UnitsDict)
                    {
                        array[i++] = item.Key;
                        array[i++] = item.Value.X;
                        array[i++] = item.Value.Y;
                    }
                    Buffer.BlockCopy(array, 0, bytes, 0, bytes.Length);
                    client.GetStream().Write(bytes, 0, bytes.Length);

                    while (true)
                    {
                        bytes = new byte[sizeof(int)];
                        if (client.GetStream().DataAvailable)
                        {
                            client.GetStream().Read(bytes, 0, sizeof(int));

                            var id = BitConverter.ToInt32(bytes, 0);
                            if (id == -1)
                                break;

                            bytes = new byte[2 * sizeof(int)];
                            array = new int[2];
                            client.GetStream().Read(bytes, 0, 2 * sizeof(int));
                            Buffer.BlockCopy(bytes, 0, array, 0, bytes.Length);

                            var targetX = array[0];
                            var targetY = array[1];

                            array = Games[client].ProcessTurn(id, targetX, targetY);
                            bytes = new byte[array.Length * sizeof(int)];
                            Buffer.BlockCopy(bytes, 0, array, 0, bytes.Length);
                            client.GetStream().Write(bytes, 0, bytes.Length);
                        }                        
                    }
                });
                client.GetStream().Dispose();
            }
        }
    }
}