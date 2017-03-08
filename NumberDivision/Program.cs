using System;
using System.Collections.Generic;

namespace NumberDivision {
    class Program {
        static void Main(string[] args) {
            List<int> list = new List<int>(new int[] { 14, 25, 21, 74, 5, 3, 4, 4, 4, 1, 5, 3 });
            int i = 8;
            NumberDivision(list, i);
            Console.ReadKey();
        }

        static void NumberDivision(IList<int> list, int i) {
            Console.WriteLine("Input number is: {0}", i);
            Dictionary<int, int> temp = new Dictionary<int, int>();
            Dictionary<int, int> multisetIndexes = new Dictionary<int, int>();

            Console.WriteLine("Input array is: ");
            for (int z = 0; z < list.Count; z++) {
                if (!temp.ContainsKey(list[z])) {
                    temp.Add(list[z], z);
                    multisetIndexes.Add(list[z], 1);
                } else {
                    multisetIndexes[list[z]]++;
                }
                Console.Write("{0} ", list[z]);
            }

            Console.WriteLine("\nPairs of array with the sum equal to input number:");
            foreach (var kvP in temp) {
                if (temp.ContainsKey(i - kvP.Key) &&
                    (i - kvP.Key > kvP.Key ||
                                                (multisetIndexes[i - kvP.Key] > 1 && i - kvP.Key == kvP.Key)))
                    Console.WriteLine("{0}, {1} ({2} duplicates)",
            kvP.Key,
            i - kvP.Key,
            i - kvP.Key == kvP.Key ? multisetIndexes[kvP.Key] * (multisetIndexes[kvP.Key] - 1) / 2 : multisetIndexes[kvP.Key] * multisetIndexes[i - kvP.Key]);
            }
        }
    }
}
