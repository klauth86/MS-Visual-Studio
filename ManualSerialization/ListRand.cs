using System.IO;

namespace ManualSerialization
{
    class ListRand
    {
        public ListNode Head;
        public ListNode Tail;
        public int Count;

        public void Serialize(FileStream s)
        {
            var current = Head;
            while (current != null)
            {
                current.Serialize(s);
                current = current.Next;
            }
        }

        public void Deserialize(FileStream s)
        {
            Clear();
            while (s.Position != s.Length)
            {
                var currentNode = new ListNode();
                currentNode.Deserialize(s);
                if (Head == null)
                    Head = currentNode;
                else
                {
                    Tail.Next = currentNode;
                    currentNode.Prev = Tail;
                }
                Tail = currentNode;
                Count++;
            }
        }

        private void Clear()
        {
            Head = null;
            Tail = null;
            Count = 0;
        }
    }
}
