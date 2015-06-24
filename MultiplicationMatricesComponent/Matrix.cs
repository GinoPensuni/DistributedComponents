using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplicationMatricesComponent
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

        public static Matrix Multiply(Matrix matrixOne, Matrix matrixTwo)
        {
            if (CheckDimensions(matrixOne, matrixTwo))
            {
            int[,] product = new int[matrixOne.RowCount, matrixTwo.ColumnCount];

            for (int i = 0; i < matrixOne.RowCount; i++)
            {
                
                for (int j = 0; j < matrixTwo.ColumnCount; j++)
                {

                    int tmp = 0;

                    for (int k = 0; k < matrixTwo.RowCount; k++)
                    {

                        tmp = tmp + (matrixOne._Matrix[i, k] * matrixTwo._Matrix[k, j]);
                    }

                    product[i,j] = tmp;
                }
            }

            return new Matrix(product);
			}
            else
            {
                throw new ArgumentException("The numbers of columns of matrix one and the number of rows of matrix two must be the same for the multiplication of matrices!");
            }
        }

        public static bool CheckDimensions(Matrix matrixOne, Matrix matrixTwo)
        {
            if (matrixOne.ColumnCount == matrixTwo.RowCount)
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
