using System;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleQueue
{
    class Program
    {
        static void Main(string[] args)
        {
            MyQueue<int> m = new MyQueue<int>();
            Task t1 = new Task(() => { fun1(m); });
            Task t2 = new Task(() => { fun2(m); });
            Task t3 = new Task(() => { fun3(m); });
            Task t4 = new Task(() => { fun4(m); });
            Task t5 = new Task(() => { fun5(m); });
            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();
            t5.Start();

            Console.ReadKey();
        }

        static async void fun1(MyQueue<int> p)
        {
            Console.WriteLine("FUN 1 BEGIN {0}", DateTime.Now);
            Console.WriteLine(p.Dequeue());
            Console.WriteLine("FUN 1 END {0}", DateTime.Now);
        }

        static async void fun2(MyQueue<int> p)
        {
            Console.WriteLine("FUN 2 BEGIN {0}", DateTime.Now);
            await Task.Delay(TimeSpan.FromSeconds(1));
            p.Enqueue(1);
            Console.WriteLine("FUN 2 END {0}", DateTime.Now);
        }

        static async void fun3(MyQueue<int> p)
        {
            Console.WriteLine("FUN 3 BEGIN {0}", DateTime.Now);
            await Task.Delay(TimeSpan.FromSeconds(3));
            Console.WriteLine(p.Dequeue());
            Console.WriteLine("FUN 3 END {0}", DateTime.Now);
        }

        static async void fun4(MyQueue<int> p)
        {
            Console.WriteLine("FUN 4 BEGIN {0}", DateTime.Now);
            await Task.Delay(TimeSpan.FromSeconds(1));
            p.Enqueue(2);
            Console.WriteLine("FUN 4 END {0}", DateTime.Now);
        }

        static async void fun5(MyQueue<int> p)
        {
            Console.WriteLine("FUN 5 BEGIN {0}", DateTime.Now);
            await Task.Delay(TimeSpan.FromSeconds(5));
            Console.WriteLine(p.Dequeue());
            Console.WriteLine("FUN 5 END {0}", DateTime.Now);
        }
    }
}
