using System;
using System.Collections.Generic;
using System.Linq;

namespace TravelNotes
{
    class Program
    {
        static void Main(string[] args)
        {
            #region SIMPLE INPUT TEST
            Console.WriteLine("SIMPLE INPUT TEST");
            try
            {
                List<KeyValuePair<string, string>> input = new List<KeyValuePair<string, string>>();
                input.Add(new KeyValuePair<string, string>("Мельбурн", "Кельн"));
                input.Add(new KeyValuePair<string, string>("Москва", "Париж"));
                input.Add(new KeyValuePair<string, string>("Кельн", "Москва"));

                List<KeyValuePair<string, string>> output = GetOrderedList(input);
                foreach (var kvp in output)
                {
                    Console.WriteLine("{0}->{1}", kvp.Key, kvp.Value);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error! {0}", e.Message);
            }
            #endregion

            #region EMPTY INPUT TEST
            Console.WriteLine("EMPTY INPUT TEST");
            try
            {
                List<KeyValuePair<string, string>> input = new List<KeyValuePair<string, string>>();
                List<KeyValuePair<string, string>> output = GetOrderedList(input);
                foreach (var kvp in output)
                {
                    Console.WriteLine("{0}->{1}", kvp.Key, kvp.Value);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error! {0}", e.Message);
            }
            #endregion

            #region NULL KEY INPUT TEST
            Console.WriteLine("NULL KEY INPUT TEST");
            try
            {
                List<KeyValuePair<string, string>> input = new List<KeyValuePair<string, string>>();
                input.Add(new KeyValuePair<string, string>(null, "Кельн"));
                List<KeyValuePair<string, string>> output = GetOrderedList(input);
                foreach (var kvp in output)
                {
                    Console.WriteLine("{0}->{1}", kvp.Key, kvp.Value);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error! {0}", e.Message);
            }
            #endregion

            #region NULL VALUE INPUT TEST
            Console.WriteLine("NULL VALUE INPUT TEST");
            try
            {
                List<KeyValuePair<string, string>> input = new List<KeyValuePair<string, string>>();
                input.Add(new KeyValuePair<string, string>("Кельн", null));
                List<KeyValuePair<string, string>> output = GetOrderedList(input);
                foreach (var kvp in output)
                {
                    Console.WriteLine("{0}->{1}", kvp.Key, kvp.Value);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error! {0}", e.Message);
            }
            #endregion

            #region SIMPLE CYCLE TEST
            Console.WriteLine("SIMPLE CYCLE TEST");
            try
            {
                List<KeyValuePair<string, string>> input = new List<KeyValuePair<string, string>>();
                input.Add(new KeyValuePair<string, string>("Мельбурн", "Кельн"));
                input.Add(new KeyValuePair<string, string>("Москва", "Мельбурн"));
                input.Add(new KeyValuePair<string, string>("Кельн", "Москва"));

                List<KeyValuePair<string, string>> output = GetOrderedList(input);
                foreach (var kvp in output)
                {
                    Console.WriteLine("{0}->{1}", kvp.Key, kvp.Value);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error! {0}", e.Message);
            }
            #endregion


            Console.ReadKey();
        }

        static List<KeyValuePair<string, string>> GetOrderedList(List<KeyValuePair<string, string>> unorderedList)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            Dictionary<string, int> dictIndex = new Dictionary<string, int>();

            foreach (var kvp in unorderedList)
            {
                dict.Add(kvp.Key, kvp.Value);
                dictIndex.Add(kvp.Key, 0);
            }

            foreach (var kvp in unorderedList)
            {
                if (dictIndex.ContainsKey(kvp.Value))
                    dictIndex[kvp.Value]++;
            }

            string rootKey = dictIndex.Where(x => x.Value == 0).FirstOrDefault().Key;
            if (rootKey == null)
                throw new Exception("There is no Root element. Wrong input data...");

            var orderedValues = new List<KeyValuePair<string, string>>();
            while (dict.ContainsKey(rootKey))
            {
                orderedValues.Add(new KeyValuePair<string, string>(rootKey, dict[rootKey]));
                rootKey = dict[rootKey];
            }
            return orderedValues;
        }
    }
}
