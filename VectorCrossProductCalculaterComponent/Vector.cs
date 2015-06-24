using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorCrossProductCalculaterComponent
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

        public static Vector CrossProduct(Vector first, Vector second)
        {
            if (CheckDimensions(first, second))
            {
                int[] result = new int[first.RowCount];

                int firstNumber = first._Vector[1] * second._Vector[2] - first._Vector[2] * second._Vector[1];

                int secondNumber = first._Vector[3] * second._Vector[0] - first._Vector[0] * second._Vector[2];

                int thirdNumber = first._Vector[0] * second._Vector[1] - first._Vector[1] * second._Vector[0];

                result[0] = firstNumber;
                result[1] = secondNumber;
                result[2] = thirdNumber;

                return new Vector(result);
            }
            else
            {
                throw new ArgumentException("Couldn't calculate cross product! The cross product is only defined for 3 dimensional vectors.");
            }
        }

        public static bool CheckDimensions(Vector first, Vector second)
        {
            if (first.RowCount == 3 && second.RowCount == 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
