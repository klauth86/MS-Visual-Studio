using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace TcpEngine {
    public class ServerHelper {
        private bool _isRunning;
        private readonly TextWriter _logger;
        private readonly TcpListener _tcpListener;

        public ServerHelper(TextWriter logger, string ip = "127.0.0.1", int port = 2275) {
            _tcpListener = new TcpListener(IPAddress.Parse(ip), port);
            _logger = logger;
            WriteLine($"[SRV]:\tServer created at {ip}:{port}");
        }

        public async Task Start() {
            _isRunning = true;

            _tcpListener.Start();
            _logger.WriteLine("[SRV]:\tServer started!");
            while (_isRunning) {
                var tcpClient = _tcpListener.AcceptTcpClientAsync();
                if (tcpClient?.Result != null)
                    CreateGame(tcpClient.Result);
            }
            _logger.WriteLine("[SRV]:\tServer stopped!");
            _tcpListener.Stop();
        }

        public void Stop() {
            _isRunning = false;
        }

        private async Task CreateGame(TcpClient tcpClient) {
            using (var game = new Game(tcpClient, _logger))
                await game.Start();
        }

        private void WriteLine(string msg) {
            _logger?.WriteLine(msg);
        }
    }
}