﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubtractionOfTwoMatricesComponent
{
    public class Matrix
    {
        private int rowCount;

        private int columnCount;

        public Matrix(int[,] matrix)
        {
            this.RowCount = matrix.GetLength(0);
            this._Matrix = matrix;
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
            get;
            private set;
        }

        public static Matrix SubtractMatrices(Matrix matrixOne, Matrix matrixTwo)
        {
            if (CheckDimensions(matrixOne, matrixTwo))
            {

                int[,] result = new int[matrixOne.RowCount, matrixTwo.ColumnCount];

                for (int i = 0; i < matrixOne.RowCount; i++)
                {
                    for (int j = 0; j < matrixTwo.ColumnCount; j++)
                    {
                        result[i, j] = matrixOne._Matrix[i, j] - matrixTwo._Matrix[i, j];
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

        public override string ToString()
        {
            string matrix = "[";

            for (int i = 0; i < this._Matrix.GetLength(0); i++)
            {
                for (int j = 0; j < this._Matrix.GetLength(1); j++)
                {
                    if (j == this._Matrix.GetLength(1) - 1 && i == this._Matrix.GetLength(0) - 1)
                    {
                        matrix = matrix + this._Matrix[i, j].ToString() + "]";
                    }
                    else if (j == this._Matrix.GetLength(1) - 1)
                    {
                        matrix = matrix + this._Matrix[i, j].ToString();
                    }
                    else
                    {
                        matrix = matrix + this._Matrix[i, j].ToString() + ",";
                    }
                }
                if (i != this._Matrix.GetLength(0) - 1)
                {
                    matrix = matrix + ";";
                }
            }

            return matrix;
        }
    }
}