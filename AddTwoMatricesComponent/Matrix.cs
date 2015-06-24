using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddTwoMatricesComponent
{
    class Matrix
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

        public static Matrix AddMatrices(Matrix matrixOne, Matrix matrixTwo)
        {
            if (CheckDimensions(matrixOne, matrixTwo))
            {

                int[,] result = new int[matrixOne.RowCount, matrixTwo.ColumnCount];

                for (int i = 0; i < matrixOne.RowCount; i++)
                {
                    for (int j = 0; j < matrixTwo.ColumnCount; j++)
                    {
                        result[i, j] = matrixOne._Matrix[i, j] + matrixTwo._Matrix[i, j];
                    }
                }

                return new Matrix(result);
            }
            else
            {
                throw new ArgumentException("The dimension of the matrices must be the same!");
            }
        }

        public static bool CheckDimensions(Matrix matrixOne, Matrix matrixTwo)
        {
            if (matrixOne.RowCount == matrixTwo.RowCount && matrixOne.ColumnCount == matrixTwo.ColumnCount)
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
