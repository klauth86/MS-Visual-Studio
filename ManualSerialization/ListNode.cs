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
            byte[] dataBytes = BitConverterHelper.StringToByteArray(Data);
            var lengthBytes = BitConverter.GetBytes(dataBytes.Length);
            s.Write(lengthBytes, 0, BitConverterHelper.IntBytesLength);
            s.Write(dataBytes, 0, dataBytes.Length);
        }

        public void Deserialize(FileStream s)
        {
            byte[] bytes = new byte[BitConverterHelper.IntBytesLength];
            s.Read(bytes, 0, BitConverterHelper.IntBytesLength);
            int length = BitConverterHelper.ByteArrayToInt(bytes);

            if (length == 0)
                return;

            bytes = new byte[length];
            s.Read(bytes, 0, length);

            Data = BitConverterHelper.ByteArrayToString(bytes);
        }
    }
}
