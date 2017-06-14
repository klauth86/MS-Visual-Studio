using System;

namespace ManualSerialization
{
    static class BitConverterHelper
    {
        public static int IntBytesLength = BitConverter.GetBytes(int.MaxValue).Length;

        public static byte[] StringToByteArray(string s)
        {
            return System.Text.Encoding.Default.GetBytes(s);
        }

        public static string ByteArrayToString(byte[] array)
        {
            return System.Text.Encoding.Default.GetString(array);
        }

        public static byte[] IntToByteArray(int i)
        {
            return BitConverter.GetBytes(i);
        }

        public static int ByteArrayToInt(byte[] array)
        {
            return BitConverter.ToInt32(array, 0);
        }
    }
}
