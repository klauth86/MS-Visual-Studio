using System;

namespace TcpServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                ServerHelper.StartServer();
                ServerHelper.Listen();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Start Server faulted");
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }
    }
}