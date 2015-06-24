using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateVectorOfPointsComponent
{
    class Vector
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

        /// <summary>
        /// Creates a vector of two points.
        /// </summary>
        /// <param name="p1">First Point</param>
        /// <param name="p2">Second Point</param>
        /// <returns></returns>
        public static Vector CreateVector(Point p1, Point p2)
        {
            int[] result = new int[p1.Point.Length];

            for (int i = 0; i < p1.Point.Length; i++)
			{
                result[i] = p2.Point[i] - p1.Point[i];
			}

            return new Vector(result);
        }
    }
}
