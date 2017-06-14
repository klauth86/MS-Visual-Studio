using System;
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
            var counterBytes = BitConverter.GetBytes(Count);
            s.Write(counterBytes, 0, BitConverterHelper.IntBytesLength);

            var current = Head;
            while (current != null)
            {
                var randIndexBytes = BitConverter.GetBytes(GetRandIndex(current.Rand));
                s.Write(randIndexBytes, 0, BitConverterHelper.IntBytesLength);
                current.Serialize(s);
                current = current.Next;
            }
        }

        private int GetRandIndex(ListNode node)
        {
            int counter = -1;
            var current = Head;
            while (current != null)
            {
                counter++;
                if (current == node)
                    return counter;
                current = current.Next;
            }
            return counter;
        }

        public void Deserialize(FileStream s)
        {
            Clear();

            byte[] counterBytes = new byte[BitConverterHelper.IntBytesLength];
            s.Read(counterBytes, 0, BitConverterHelper.IntBytesLength);
            Count = BitConverterHelper.ByteArrayToInt(counterBytes);

            var counter = 0;
            var randIndice = new int[Count];

            while (s.Position != s.Length)
            {
                byte[] bytes = new byte[BitConverterHelper.IntBytesLength];
                s.Read(bytes, 0, BitConverterHelper.IntBytesLength);
                randIndice[counter] = BitConverterHelper.ByteArrayToInt(bytes);

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
                counter++;
            }

            var current = Head;
            foreach (var item in randIndice)
            {
                if (item>=0)
                {
                    current.Rand = GetRandByIndex(item);
                }
                current = current.Next;
            }
        }

        private ListNode GetRandByIndex(int item)
        {
            ListNode result = Head;
            while (item-- > 0)
                result = result.Next;
            return result;
        }

        private void Clear()
        {
            Head = null;
            Tail = null;
            Count = 0;
        }
    }
}
