using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleCacheStore
{
    class NotSerializableClass
    {
        public int x;
        public int y;

        public NotSerializableClass(int xx, int yy)
        {
            x = xx;
            y = yy;
        }

        public override string ToString()
        {
            return string.Format("NotSerializableClass[{0},{1}]", x, y);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            MyCache_Add_Test();
            Console.WriteLine();

            MyCache_Get_Test();
            Console.WriteLine();

            MyCache_Remove_Test();
            Console.WriteLine();

            MyCache_Multithreading_Test();
            Console.WriteLine();

            Console.ReadKey();
        }

        static void MyCache_Add_Test()
        {
            Console.WriteLine("MyCache_Add_Test: BEGIN");
            try
            {
                Console.WriteLine("EXECUTING MyCache.Instance.Add(\"1\", \"11\")");
                MyCache.Instance.Add("1", "11");
                Console.WriteLine("SUCCESS");
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR {0}", e.Message);
            }

            try
            {
                Console.WriteLine("EXECUTING MyCache.Instance.Add(\"2\", \"22\", STORAGE.DISK)");
                MyCache.Instance.Add("2", "22", STORAGE.DISK);
                Console.WriteLine("SUCCESS");
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR {0}", e.Message);
            }

            try
            {
                Console.WriteLine("EXECUTING MyCache.Instance.Add(\"3\", new List<int>(new int[] { 1, 2, 3 }), STORAGE.MEMORY, new DateTime(2020, 2, 1, 11, 22, 01))");
                MyCache.Instance.Add("3", new List<int>(new int[] { 1, 2, 3 }), STORAGE.MEMORY, new DateTime(2020, 2, 1, 11, 22, 01));
                Console.WriteLine("SUCCESS");
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR {0}", e.Message);
            }

            try
            {
                Console.WriteLine("EXECUTING MyCache.Instance.Add(\"1\", 1.25d, STORAGE.MEMORY, new DateTime(2020, 2, 1, 11, 22, 01)");
                MyCache.Instance.Add("1", 1.25d, STORAGE.MEMORY, new DateTime(2020, 2, 1, 11, 22, 01));
                Console.WriteLine("ERROR Double input of key \"1\"");
            }
            catch (Exception e)
            {
                Console.WriteLine("SUCCESS {0}", e.Message);
            }

            try
            {
                Console.WriteLine("EXECUTING MyCache.Instance.Add(\"4\", new NotSerializableClass(1, 2), STORAGE.DISK, new DateTime(2020, 2, 1, 11, 22, 01))");
                MyCache.Instance.Add("4", new NotSerializableClass(1, 2), STORAGE.DISK, new DateTime(2020, 2, 1, 11, 22, 01));
                Console.WriteLine("ERROR Store not serializable object on DiSK storage");
            }
            catch (Exception e)
            {
                Console.WriteLine("SUCCESS {0}", e.Message);
            }

            try
            {
                Console.WriteLine("EXECUTING MyCache.Instance.Add(\"4\", new NotSerializableClass(1, 2), STORAGE.MEMORY, new DateTime(2020, 2, 1, 11, 22, 01))");
                MyCache.Instance.Add("4", new NotSerializableClass(1, 2), STORAGE.MEMORY, new DateTime(2020, 2, 1, 11, 22, 01));
                Console.WriteLine("SUCCESS");
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR {0}", e.Message);
            }

            try
            {
                Console.WriteLine("EXECUTING MyCache.Instance.Add(\"5\", null)");
                MyCache.Instance.Add("5", null);
                Console.WriteLine("ERROR Store NULL object");
            }
            catch (Exception e)
            {
                Console.WriteLine("SUCCESS {0}", e.Message);
            }

            Console.WriteLine("MyCache_Add_Test: END");
        }

        static void MyCache_Get_Test()
        {
            Console.WriteLine("MyCache_Get_Test: BEGIN");
            try
            {
                Console.WriteLine("EXECUTING MyCache.Instance.Get(null)");
                MyCache.Instance.Get(null);
                Console.WriteLine("ERROR");
            }
            catch (Exception e)
            {
                Console.WriteLine("SUCCESS {0}", e.Message);
            }

            try
            {
                Console.WriteLine("EXECUTING MyCache.Instance.Get(\"999\")");
                MyCache.Instance.Get("999");
                Console.WriteLine("ERROR");
            }
            catch (Exception e)
            {
                Console.WriteLine("SUCCESS {0}", e.Message);
            }

            try
            {
                Console.WriteLine("EXECUTING MyCache.Instance.Get(\"4\")");
                var elem = MyCache.Instance.Get("4");
                var t = elem.Key;
                var val = elem.Value;
                Convert.ChangeType(val, t);
                Console.WriteLine(val);
                Console.WriteLine("SUCCESS");
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR {0}", e.Message);
            }
            Console.WriteLine("MyCache_Get_Test: END");
        }

        static void MyCache_Remove_Test()
        {
            Console.WriteLine("MyCache_Remove_Test: BEGIN");
            try
            {
                Console.WriteLine("EXECUTING MyCache.Instance.Remove(null)");
                MyCache.Instance.Remove(null);
                Console.WriteLine("ERROR");
            }
            catch (Exception e)
            {
                Console.WriteLine("SUCCESS {0}", e.Message);
            }

            try
            {
                Console.WriteLine("EXECUTING MyCache.Instance.Remove(\"999\")");
                MyCache.Instance.Remove("999");
                Console.WriteLine("ERROR");
            }
            catch (Exception e)
            {
                Console.WriteLine("SUCCESS {0}", e.Message);
            }

            try
            {
                Console.WriteLine("EXECUTING MyCache.Instance.Remove(\"4\")");
                MyCache.Instance.Remove("4");
                Console.WriteLine("SUCCESS");
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR {0}", e.Message);
            }

            try
            {
                Console.WriteLine("EXECUTING MyCache.Instance.Remove(\"2\")");
                MyCache.Instance.Remove("2");
                Console.WriteLine("SUCCESS");
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR {0}", e.Message);
            }
            Console.WriteLine("MyCache_Remove_Test: END");
        }

        static void MyCache_Multithreading_Test()
        {
            Console.WriteLine("MyCache_Multithreading_Test: BEGIN");
            Task t1 = new Task(() => {
                try
                {
                    MyCache.Instance.Add("777", "777_Value");
                    Console.WriteLine("Task 1 Add SUCCESS");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Task 1 {0}", e.Message);
                }
            });
            Task t2 = new Task(() => {
                try
                {
                    Console.WriteLine("Task 2 {0} {1}", MyCache.Instance.Get("777").Key, MyCache.Instance.Get("777").Value);
                    Console.WriteLine("Task 2 Get SUCCESS");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Task 2 {0}", e.Message);
                }
            });


            Task t3 = new Task(() => {
                try
                {
                    MyCache.Instance.Remove("777");
                    Console.WriteLine("Task 3 Remove SUCCESS");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Task 3 {0}", e.Message);
                }
            });
            Task t4 = new Task(() => {
                try
                {
                    Console.WriteLine("Task 4 {0} {1}", MyCache.Instance.Get("777").Key, MyCache.Instance.Get("777").Value);
                    Console.WriteLine("Task 4 Get SUCCESS");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Task 4 {0}", e.Message);
                }
            });
            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();

            t1.Wait();
            t2.Wait();
            t3.Wait();
            t4.Wait();

            Console.WriteLine("MyCache_Multithreading_Test: END");
        }
    }
}
