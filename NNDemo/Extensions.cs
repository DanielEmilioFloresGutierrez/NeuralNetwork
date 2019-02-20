using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNDemo
{
    public static class Extensions
    {
        public static T[] GetColumn<T>(this T[][] matrix, int column)
        {
            column = (column<0|| column> matrix[0].Length) ? 0 : column;
            T[] output = new T[matrix.Length];
            for (int i = 0; i < matrix.Length; i++)
            {
                output[i] = matrix[i][column];
            }
            return output;
        }

        public static T[][] GetColumns<T>(this T[][] matrix, int init, int until = 0)
        {
            until = (until - init < 0) ? init : until;
            T[][] output = new T[matrix.Length][];
            for (int i = 0; i < matrix.Length; i++)
            {
                output[i] = new T[(until-init)+1];
                for (int j = init; j <= until; j++)
                {
                    output[i][j-init] = matrix[i][j];
                }
            }
            return output;
        }

        public static T MinFromColumn<T>(this T[][] matrix ,int column)where T:struct
        {
            column = (column<0||column>matrix[0].Length-1)?0:column;
            return matrix.GetColumn(column).Min();
        }

        public static T MaxFromColumn<T>(this T[][] matrix, int column) where T : struct
        {
            column = (column < 0 || column > matrix[0].Length - 1) ? 0 : column;
            return matrix.GetColumn(column).Max();
        }

        public static T[][] Transpose<T>(this T[][]matrix)
        {
            T[][] output = new T[matrix[0].Length][];
            for (int i = 0; i < output.Length; i++)
            {
                output[i] = new T[matrix.Length];
                for (int j = 0; j < output[i].Length; j++)
                {
                    output[i][j] = matrix[j][i];
                }
            }
            return output;
        }

        public static double[] Normalize(this double[] vector)
        {
            double min = vector.Min<double>();
            double max = vector.Max();
            double[] output = new double[vector.Length];
            for (int i = 0; i < vector.Length; i++)
            {
                output[i] = (vector[i] - min) / (max - min);
            }
            return output;
        }

        //This method has problems
        public static double[][] NormalizeMatrix(this double[][]matrix)
        {
            double[][] tempMatrix = new double[matrix[0].Length][];
            double[][] output = new double[matrix.Length][];
            for (int i = 0; i < matrix.Length; i++)
            {
                output[i] = new double[matrix[i].Length];
            }
            for (int column = 0; column < matrix[0].Length; column++)
            {
                tempMatrix[column] = matrix.GetColumn(0).Normalize();
            }
            return tempMatrix.Transpose();
        }

        public static void PrintMatrix<T>(this T[][] matrix )
        {
            for (int row = 0; row < matrix.Length; row++)
            {
                for (int column = 0; column < matrix[row].Length; column++)
                {
                    Console.Write(matrix[row][column].ToString()+"\t");
                }
                Console.WriteLine();
            }
        }

        public static void PrintVector<T>(this T[] vector)
        {
            for (int i = 0; i < vector.Length; i++)
            {
                Console.Write(vector[i]+"\t");
            }
            Console.WriteLine();
        }

    }

}
