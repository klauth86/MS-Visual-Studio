using System;
using System.Collections.Generic;
using GameEngine;

namespace TcpClientLib
{
    public static class ClientHelper
    {
        public static int Dimension;
        public static Dictionary<int, Unit> UnitsDict = new Dictionary<int, Unit>();

        private static System.Net.Sockets.TcpClient _tcpClient;

        public static void Start(string IP = "169.254.60.209", int port = 2275)
        {
            _tcpClient = new System.Net.Sockets.TcpClient();
            _tcpClient.Connect(IP, port);
        }

        public static void GetGame()
        {
            var bytes = new byte[sizeof(int)];
            _tcpClient.GetStream().Read(bytes, 0, sizeof(int));
            var size = BitConverter.ToInt32(bytes, 0);

            bytes = new byte[size * sizeof(int)];
            _tcpClient.GetStream().Read(bytes, 0, size * sizeof(int));
            var array = new int[size];
            Buffer.BlockCopy(bytes, 0, array, 0, size * sizeof(int));
            Dimension = array[0];
            for (int i = 1; i < size; i = i + 3)
            {
                UnitsDict.Add(array[i], new Unit(array[i + 1], array[i + 2]));
            }
        }

        public static int[] GetTurn(int id, int targetX, int targetY)
        {
            var bytes = new byte[3 * sizeof(int)];
            Buffer.BlockCopy(new int[] { id, targetX, targetY }, 0, bytes, 0, bytes.Length);
            _tcpClient.GetStream().Write(bytes, 0, 3 * sizeof(int));
            _tcpClient.GetStream().Flush();

            bytes = new byte[sizeof(int)];
            _tcpClient.GetStream().Read(bytes, 0, sizeof(int));
            var size = BitConverter.ToInt32(bytes, 0);

            if (size == -1)
                return new int[] { };

            bytes = new byte[size * sizeof(int)];
            var array = new int[size];
            _tcpClient.GetStream().Read(bytes, 0, size * sizeof(int));
            Buffer.BlockCopy(bytes, 0, array, 0, size * sizeof(int));
            return array;
        }

        public static void Stop()
        {
            _tcpClient.GetStream().Write(BitConverter.GetBytes(-1), 0, sizeof(int));
            _tcpClient.GetStream().Dispose();
            _tcpClient.Close();
        }
    }
}
