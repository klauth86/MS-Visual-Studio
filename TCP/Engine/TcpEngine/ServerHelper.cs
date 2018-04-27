using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace TcpEngine {
    public class ServerHelper {
        private bool _isRunning;
        private readonly TextWriter _logger;
        private readonly TcpListener _tcpListener;

        public ServerHelper(TextWriter logger, string ip = "169.254.60.209", int port = 2275) {
            _tcpListener = new TcpListener(IPAddress.Parse(ip), port);
            _logger = logger;
            WriteLine($"[SRV]:\tServer created at {ip}:{port}");
        }

        public void Start() {
            _isRunning = true;
            _tcpListener.Start();
            _logger.WriteLine("[SRV]:\tServer started!");
            ServerLoop();
        }

        public void Stop() {
            _isRunning = false;
            _tcpListener.Stop();
            _logger.WriteLine("[SRV]:\tServer stopped!");
        }

        private async Task ServerLoop() {
            while (_isRunning) {
                var tcpClient = await _tcpListener.AcceptTcpClientAsync();
                if (_isRunning && tcpClient != null)
                    CreateGame(tcpClient);
            }
        }

        private async Task CreateGame(TcpClient tcpClient) {
            await Task.Yield();
            using (var game = new ClientInfo(tcpClient, _logger))
                game.Start();
        }

        private void WriteLine(string msg) {
            _logger?.WriteLine(msg);
        }
    }
}