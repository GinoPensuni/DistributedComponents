using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplyVectorWithScalarComponent
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

        public int[] Vector
        {
            get
            {
                return this.vector;
            }
        }

        public static Vector MultiplyWithScalar(Vector first, int scalar)
        {
            int[] result = new int[first.Vector.Length];

            for (int i = 0; i < first.Vector.Length; i++)
            {
                result[i] = first.vector[i] * scalar;
            }

            return new Vector(result);          
        }
    }
}
