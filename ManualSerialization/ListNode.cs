using System;
using System.IO;

namespace ManualSerialization
{
    class ListNode
    {
        public ListNode Prev;
        public ListNode Next;
        public ListNode Rand; // произвольный элемент внутри списка
        public string Data;

        public void Serialize(FileStream s)
        {
            byte[] array = BitConverterHelper.StringToByteArray(Data);
            var lengthBytes = BitConverter.GetBytes(array.Length);
            s.Write(lengthBytes, 0, BitConverterHelper.IntBytesLength);
            s.Write(array, 0, array.Length);
        }

        public void Deserialize(FileStream s)
        {
            byte[] array = new byte[BitConverterHelper.IntBytesLength];
            s.Read(array, 0, BitConverterHelper.IntBytesLength);

            int length = BitConverterHelper.ByteArrayToInt(array);

            if (length == 0)
                return;

            array = new byte[length];
            s.Read(array, 0, length);

            Data = BitConverterHelper.ByteArrayToString(array);
        }
    }
}
