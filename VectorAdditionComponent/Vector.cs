using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorAdditionComponent
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

        public int [] _Vector
        {
            get
            {
                return this.vector;
            }
        }

        public static Vector Add(Vector first, Vector second)
        {
            if (CheckDimensions(first, second))
            {
                int[] result = new int[first.RowCount];

                for (int i = 0; i < first.RowCount; i++)
                {
                    result[i] = first._Vector[i] + second._Vector[i];
                }

                return new Vector(result);
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
    }
}
