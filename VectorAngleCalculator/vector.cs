using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorAngleCalculator
{
    public class Vector
    {
        private int rowCount;

        private int columnCount;

        private int[] vector;

        public Vector(int[] vector)
        {
            this.vector = vector;

            this.columnCount = 1;

            this.rowCount = vector.Length;
        }

        public int RowCount
        {
            get
            {
                return this.rowCount;
            }
        }

        public int ColumnCount
        {
            get
            {
                return this.columnCount;
            }
        }

        public int[] _Vector
        {
            get
            {
                return this.vector;
            }
        }

        public static double CalcAngle(Vector first, Vector second)
        {
            if (CheckDimensions(first, second))
            {
                double denumerator = CalculateAbsoluteValue(first) * CalculateAbsoluteValue(second);

                double numerator = Convert.ToDouble(CalculateScalar(first, second));

                double cos = numerator / denumerator;

                return Math.Acos(cos);
            }
            else
            {
                throw new ArgumentException("The number of rows of the two vectors must be the same!");
            }
        }

        public static int CalculateScalar(Vector first, Vector second)
        {
            if (CheckDimensions(first, second))
            {
                int sum = 0;

                for (int i = 0; i < first.RowCount; i++)
                {
                    sum = sum + (first.vector[i] * second.vector[i]);
                }

                return sum;
            }
            else
            {
                throw new ArgumentException("The number of rows of the two vectors must be the same!");
            }
        }

        public static bool CheckDimensions(Vector first, Vector second)
        {
            if (first.RowCount == second.RowCount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static double CalculateAbsoluteValue(Vector vector)
        {
            double square = 0;

            for (int i = 0; i < vector._Vector.Length; i++)
            {
                square = square + (Math.Pow(vector._Vector[i], 2));
            }

            return Math.Sqrt(square);
        }
    }
}
