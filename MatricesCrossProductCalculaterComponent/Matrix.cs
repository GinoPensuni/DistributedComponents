using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatricesCrossProductCalculaterComponent
{
    public class Matrix
    {
        private int rowCount;

        private int columnCount;

        private int[,] matrix;

        public Matrix(int[,] matrix)
        {
            this.RowCount = matrix.GetLength(0);

            this.ColumnCount = matrix.GetLength(1);
        }

        public int RowCount
        {
            get
            {
                return this.rowCount;
            }

            set
            {
                if (value > 0)
                {
                    this.rowCount = value;
                }
                else
                {
                    throw new ArgumentException("The number of rows can not be less than 0");
                }
            }
        }
        public int ColumnCount
        {
            get
            {
                return this.columnCount;
            }

            set
            {
                if (value > 0)
                {
                    this.columnCount = value;
                }
                else
                {
                    throw new ArgumentException("The number of columns can not be less than 0");
                }
            }
        }

        public int[,] _Matrix
        {
            get
            {
                return this.matrix;
            }
        }

        public static Matrix CrossProduct(Matrix matrixOne, Matrix matrixTwo)
        {
            if (CheckDimensions(matrixOne, matrixTwo))
            {
                int[,] product = new int[matrixOne.RowCount, matrixTwo.ColumnCount];

                int[] firstRow = 
                }

                return new Matrix(product);
            }
            else
            {
                throw new ArgumentException("Can't calculate cross product, the cross product is only defined for 3 dimensional matrices!");
            }
        }

        public static bool CheckDimensions(Matrix matrixOne, Matrix matrixTwo)
        {
            if (matrixOne.matrix.GetLength(0) == 3 && matrixOne.matrix.GetLength(1) == 3 && matrixTwo.matrix.GetLength(0) == 3 && matrixTwo.matrix.GetLength(1) == 3)
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
