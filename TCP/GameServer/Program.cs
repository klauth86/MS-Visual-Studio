using System;
using TcpEngine;

namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            try {
                var srv = new ServerHelper(Console.Out);
                srv.StartAsync().Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine("Start Server faulted");
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }
    }
}
