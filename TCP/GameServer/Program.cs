using System;
using TcpEngine;

namespace GameServer {
    class Program {
        static void Main() {
            ServerHelper srv = null;
            Help();
            while (true) {
                try {
                    var line = Console.ReadLine();
                    switch (line?.ToLower()) {
                        case "help":
                            break;
                        case "create":
                            srv?.Stop();
                            srv = new ServerHelper(Console.Out);
                            break;
                        case "start":
                            if (srv == null)
                                throw new NullReferenceException("You must create srv before!");
                            srv.Start();
                            break;
                        case "stop":
                            if (srv == null)
                                throw new NullReferenceException("You must create srv before!");
                            srv.Stop();
                            break;
                    }
                }
                catch (Exception e) {
                    Console.WriteLine("------------------------------------------------------------------");
                    Console.WriteLine($"Operation faulted: {e.Message}");
                    Console.WriteLine("------------------------------------------------------------------");
                }
            }
        }

        private static void Help() {
            Console.WriteLine("------------------------------------------------------------------");
            Console.WriteLine("Commands:");
            Console.WriteLine("create - stop current (if needed) and start new server");
            Console.WriteLine("start - start current server");
            Console.WriteLine("stop - stop current server");
            Console.WriteLine("------------------------------------------------------------------");
        }
    }
}