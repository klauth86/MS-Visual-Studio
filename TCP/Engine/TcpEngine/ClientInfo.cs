using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using GameEngine;

namespace TcpEngine {
    class ClientInfo : IDisposable {
        private NetworkStream _stream;
        private TextWriter _logger;
        private Engine _engine;

        public ClientInfo(TcpClient tcpClient, TextWriter logger) {
            _stream = tcpClient.GetStream();
            _logger = logger;
            _engine = new Engine();
        }

        public void Dispose() {
            _stream.Dispose();
        }

        internal void Start() {
            var intLength = 1 + 3 * _engine.Units.Count;
            var bytes = new byte[(intLength + 1) * sizeof(int)];
            var array = new int[intLength + 1];

            array[0] = intLength;
            array[1] = _engine.Dimension;
            int i = 2;
            foreach (var item in _engine.Units) {
                array[i++] = item.Key;
                array[i++] = item.Value.X;
                array[i++] = item.Value.Y;
            }
            Buffer.BlockCopy(array, 0, bytes, 0, bytes.Length);
            _stream.Write(bytes, 0, bytes.Length);

            while (true) {
                if (_stream.DataAvailable) {
                    bytes = new byte[sizeof(int)];
                    _stream.Read(bytes, 0, sizeof(int));

                    var id = BitConverter.ToInt32(bytes, 0);
                    if (id == -1)
                        break;

                    bytes = new byte[2 * sizeof(int)];
                    array = new int[2];
                    _stream.Read(bytes, 0, 2 * sizeof(int));
                    Buffer.BlockCopy(bytes, 0, array, 0, bytes.Length);

                    array = _engine.EvalTurn(id, array[0], array[1]);
                    if (array[0] == -1)
                        continue;

                    _stream.Write(BitConverter.GetBytes(array[0]), 0, sizeof(int));

                    bytes = new byte[(array.Length - 1) * sizeof(int)];
                    Buffer.BlockCopy(array, sizeof(int), bytes, 0, bytes.Length);
                    _stream.Write(bytes, 0, bytes.Length);
                }
            }
        }
    }
}