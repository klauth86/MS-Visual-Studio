using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using GameEngine;

namespace TcpEngine {
    public class ServerHelper {
        private readonly TextWriter _logger;
        private readonly TcpListener _tcpListener;
        private readonly Dictionary<TcpClient, Engine> _games;

        private bool _isListening;

        public ServerHelper(TextWriter logger, string ip = "127.0.0.1", int port = 2275) {
            _tcpListener = new TcpListener(IPAddress.Parse(ip), port);
            _tcpListener.Start();

            _logger = logger;
            _games = new Dictionary<TcpClient, Engine>();
        }

        public async Task StartAsync() {
            if (_isListening)
                throw new InvalidOperationException("Server is already running!");
            _isListening = true;

            WriteLine("Server started!");

            while (_isListening) {
                await Task.Delay(100);
                await _listen();
            }
        }

        public async Task StopAsync() {
            if (!_isListening)
                throw new InvalidOperationException("Server is not running!");
            _isListening = false;

            WriteLine("Server stopped!");

            foreach (var client in _games.Keys.ToList()) {
                client.GetStream().Dispose();
                client.Close();
                _games.Remove(client);
            }
            _tcpListener.Stop();
        }

        private async Task _listen() {
            var clientTask = _tcpListener.AcceptTcpClientAsync();

            if (clientTask.Result != null) {
                var client = clientTask.Result;
                await Task.Run(() => {
                    _games.Add(client, new Engine());

                    var intLength = 1 + 3 * _games[client].Units.Count;
                    var bytes = new byte[(intLength + 1) * sizeof(int)];
                    var array = new int[intLength + 1];

                    array[0] = intLength;
                    array[1] = _games[client].Dimension;
                    int i = 2;
                    foreach (var item in _games[client].Units) {
                        array[i++] = item.Key;
                        array[i++] = item.Value.X;
                        array[i++] = item.Value.Y;
                    }
                    Buffer.BlockCopy(array, 0, bytes, 0, bytes.Length);
                    client.GetStream().Write(bytes, 0, bytes.Length);

                    while (true) {
                        bytes = new byte[sizeof(int)];
                        if (client.GetStream().DataAvailable) {
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

                            array = _games[client].EvalTurn(id, targetX, targetY);
                            bytes = new byte[array.Length * sizeof(int)];
                            Buffer.BlockCopy(bytes, 0, array, 0, bytes.Length);
                            client.GetStream().Write(bytes, 0, bytes.Length);
                        }
                    }
                });
                client.GetStream().Dispose();
            }
        }

        private void WriteLine(string msg) {
            _logger?.WriteLine(msg);
        }
    }
}