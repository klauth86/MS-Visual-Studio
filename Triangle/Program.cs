using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Triangle
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Egyptian triangle test
            Console.WriteLine("Egyptian triangle test");
            Console.WriteLine("Input values {0}, {1}, {2}...", 5, 3, 4);
            try { Console.WriteLine(S(5, 3, 4)); }
            catch(Exception e) { Console.WriteLine(e.Message); }
            #endregion

            #region Zero side test 1
            Console.WriteLine("Zero side test");
            Console.WriteLine("Input values {0}, {1}, {2}...", 0, 3, 4);
            try { Console.WriteLine(S(0, 3, 4)); }
            catch (Exception e) { Console.WriteLine(e.Message); }
            #endregion

            #region Zero side test 2
            Console.WriteLine("Zero side test");
            Console.WriteLine("Input values {0}, {1}, {2}...", 1, 0, 4);
            try { Console.WriteLine(S(1, 0, 4)); }
            catch (Exception e) { Console.WriteLine(e.Message); }
            #endregion

            #region Zero side test 3
            Console.WriteLine("Zero side test");
            Console.WriteLine("Input values {0}, {1}, {2}...", 1, 3, 0);
            try { Console.WriteLine(S(1, 3, 0)); }
            catch (Exception e) { Console.WriteLine(e.Message); }
            #endregion

            #region Negative side test 1
            Console.WriteLine("Negative side test");
            Console.WriteLine("Input values {0}, {1}, {2}...", -5, 3, 4);
            try { Console.WriteLine(S(-5, 3, 4)); }
            catch (Exception e) { Console.WriteLine(e.Message); }
            #endregion

            #region Negative side test 2
            Console.WriteLine("Negative side test");
            Console.WriteLine("Input values {0}, {1}, {2}...", 5, -3, 4);
            try { Console.WriteLine(S(5, -3, 4)); }
            catch (Exception e) { Console.WriteLine(e.Message); }
            #endregion

            #region Negative side test 3
            Console.WriteLine("Negative side test");
            Console.WriteLine("Input values {0}, {1}, {2}...", 5, 3, -4);
            try { Console.WriteLine(S(5, 3, -4)); }
            catch (Exception e) { Console.WriteLine(e.Message); }
            #endregion

            #region Nontriangle test
            Console.WriteLine("Nontriangle test");
            Console.WriteLine("Input values {0}, {1}, {2}...", 5, 3, 18);
            try { Console.WriteLine(S(5, 3, 18)); }
            catch (Exception e) { Console.WriteLine(e.Message); }
            #endregion

            #region Nontriangle test 2
            Console.WriteLine("Nontriangle test");
            Console.WriteLine("Input values {0}, {1}, {2}...", 5, 3, 8);
            try { Console.WriteLine(S(5, 3, 8)); }
            catch (Exception e) { Console.WriteLine(e.Message); }
            #endregion

            Console.ReadKey();
        }

        static double S(double a, double b, double c)
        {
            if (a <= 0 || b <= 0 || c <= 0)
                throw new Exception("Error! Triangle can't have side with nonpositive length...");

            double s = 0.25 * Math.Sqrt((a + b + c) * (a + b - c) * (a + c - b) * (b + c - a));
            if (s==0 || double.IsNaN(s))
                throw new Exception(string.Format("Error! Triangle can't have sides with lengths {0}, {1}, {2}...", a, b, c));
            return s;
        }
    }
}
