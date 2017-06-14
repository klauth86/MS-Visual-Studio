using System.IO;

namespace ManualSerialization
{
    class Program
    {
        static void Main(string[] args)
        {
            var lst = new ListRand();
            lst.Head = new ListNode();
            lst.Head.Data = "AaAaAa";
            lst.Head.Next = new ListNode();
            lst.Head.Next.Data = "222222";
            lst.Head.Next.Next = new ListNode();
            lst.Head.Next.Next.Data = "CCCCCC33333";

            lst.Head.Next.Next.Rand = lst.Head;
            lst.Head.Rand = lst.Head.Next;

            lst.Count = 3;

            var textfile = "1.bin";
            using (var fs = File.Create(textfile))
            {
                lst.Serialize(fs);
            }

            var lst2 = new ListRand();
            using (var fs = File.Open(textfile, FileMode.Open))
            {
                lst2.Deserialize(fs);
            }
        }
    }
}
