using System;
using System.Collections.Generic;

namespace GameEngine {
    public class Engine {

        public static Random Generator = new Random();

        #region Props

        public int Dimension { get; }
        public Dictionary<int, Unit> Units { get; }

        #endregion

        #region Ctors

        public Engine() {
            // Board dimension
            Dimension = Generator.Next(7, 13);
            Console.WriteLine($"Dimension {Dimension}");

            // Units creation and positioning
            var numOfUnits = Generator.Next(1, 6);
            Units = new Dictionary<int, Unit>(numOfUnits);
            Console.WriteLine($"numOfUnits {numOfUnits}");

            var positions = new List<int>();
            for (int i = 0; i < Dimension * Dimension; i++) {
                positions.Add(i);
            }

            for (int i = 0; i < numOfUnits; i++) {
                var index = Generator.Next(0, Dimension * Dimension - i);
                var position = positions[index];
                positions.RemoveAt(index);
                Units.Add(i, new Unit(position % Dimension, position / Dimension));
                Console.WriteLine($"{{{i}, [{position % Dimension}, {position / Dimension}]}}");
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Eval Turn of Unit 
        /// </summary>
        /// <param name="id">Unit Id</param>
        /// <param name="targetX">Target point X coord</param>
        /// <param name="targetY">Target point Y coord</param>
        /// <returns></returns>
        public int[] EvalTurn(int id, int targetX, int targetY) {
            var A = new int[Dimension * Dimension];
            foreach (var unit in Units.Values) {
                A[unit.X + unit.Y * Dimension] = -1;
                ;
            }
            WaveAlg(A, Units[id].X, Units[id].Y);

            for (int i = 0; i < Dimension; i++) {
                for (int j = 0; j < Dimension; j++) {
                    Console.Write($"{A[i + j * Dimension]} ");
                }
                Console.WriteLine();
            }

            if (A[targetX + targetY * Dimension] == 0)
                return new int[] {-1};

            Units[id].X = targetX;
            Units[id].Y = targetY;

            var result = new int[2 * A[targetX + targetY * Dimension] + 1];
            result[0] = 2 * A[targetX + targetY * Dimension];
            var x = targetX;
            var y = targetY;

            for (int i = A[targetX + targetY * Dimension]; i > 0; i--) {
                result[2 * i] = y;
                result[2 * i - 1] = x;

                if (IsBounded(x + 1) && A[x + 1 + y * Dimension] == A[x + y * Dimension] - 1) {
                    x = x + 1;
                    continue;
                }

                if (IsBounded(x - 1) && A[x - 1 + y * Dimension] == A[x + y * Dimension] - 1) {
                    x = x - 1;
                    continue;
                }

                if (IsBounded(x + 1) && IsBounded(y - 1) &&
                    A[x + 1 + (y - 1) * Dimension] == A[x + y * Dimension] - 1) {
                    x = x + 1;
                    y = y - 1;
                    continue;
                }

                if (IsBounded(x - 1) && IsBounded(y - 1) &&
                    A[x - 1 + (y - 1) * Dimension] == A[x + y * Dimension] - 1) {
                    x = x - 1;
                    y = y - 1;
                    continue;
                }

                if (IsBounded(x + 1) && IsBounded(y + 1) &&
                    A[x + 1 + (y + 1) * Dimension] == A[x + y * Dimension] - 1) {
                    x = x + 1;
                    y = y + 1;
                    continue;
                }

                if (IsBounded(x - 1) && IsBounded(y + 1) &&
                    A[x - 1 + (y + 1) * Dimension] == A[x + y * Dimension] - 1) {
                    x = x - 1;
                    y = y + 1;
                    continue;
                }

                if (IsBounded(y + 1) && A[x + (y + 1) * Dimension] == A[x + y * Dimension] - 1) {
                    y = y + 1;
                    continue;
                }

                if (IsBounded(y - 1) && A[x + (y - 1) * Dimension] == A[x + y * Dimension] - 1) {
                    y = y - 1;
                    continue;
                }
            }

            return result;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Fresnel light method to measure distance 
        /// </summary>
        /// <param name="a">Game matrix</param>
        /// <param name="startX">Start point X coord</param>
        /// <param name="startY">Start point Y coord</param>
        private void WaveAlg(int[] a, int startX, int startY) {
            Queue<int> queue = new Queue<int>();
            a[startX + startY * Dimension] = 1;
            queue.Enqueue(startX + startY * Dimension);

            while (queue.Count > 0) {
                var current = queue.Dequeue();
                var x = current % Dimension;
                var y = current / Dimension;

                if (IsBounded(x + 1) && a[x + 1 + y * Dimension] == 0) {
                    a[x + 1 + y * Dimension] = a[x + y * Dimension] + 1;
                    queue.Enqueue(x + 1 + y * Dimension);
                }

                if (IsBounded(x - 1) && a[x - 1 + y * Dimension] == 0) {
                    a[x - 1 + y * Dimension] = a[x + y * Dimension] + 1;
                    queue.Enqueue(x - 1 + y * Dimension);
                }

                if (IsBounded(x + 1) && IsBounded(y - 1) && a[x + 1 + (y - 1) * Dimension] == 0) {
                    a[x + 1 + (y - 1) * Dimension] = a[x + y * Dimension] + 1;
                    queue.Enqueue(x + 1 + (y - 1) * Dimension);
                }

                if (IsBounded(x - 1) && IsBounded(y - 1) && a[x - 1 + (y - 1) * Dimension] == 0) {
                    a[x - 1 + (y - 1) * Dimension] = a[x + y * Dimension] + 1;
                    queue.Enqueue(x - 1 + (y - 1) * Dimension);
                }

                if (IsBounded(x + 1) && IsBounded(y + 1) && a[x + 1 + (y + 1) * Dimension] == 0) {
                    a[x + 1 + (y + 1) * Dimension] = a[x + y * Dimension] + 1;
                    queue.Enqueue(x + 1 + (y + 1) * Dimension);
                }

                if (IsBounded(x - 1) && IsBounded(y + 1) && a[x - 1 + (y + 1) * Dimension] == 0) {
                    a[x - 1 + (y + 1) * Dimension] = a[x + y * Dimension] + 1;
                    queue.Enqueue(x - 1 + (y + 1) * Dimension);
                }

                if (IsBounded(y + 1) && a[x + (y + 1) * Dimension] == 0) {
                    a[x + (y + 1) * Dimension] = a[x + y * Dimension] + 1;
                    queue.Enqueue(x + (y + 1) * Dimension);
                }

                if (IsBounded(y - 1) && a[x + (y - 1) * Dimension] == 0) {
                    a[x + (y - 1) * Dimension] = a[x + y * Dimension] + 1;
                    queue.Enqueue(x + (y - 1) * Dimension);
                }
            }
        }

        /// <summary>
        /// Check the bounds of point
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private bool IsBounded(int x) {
            return x >= 0 && x < Dimension;
        }

        #endregion
    }
}