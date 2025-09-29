using System;

namespace IJuniorTasks
{
    internal class Program32
    {
        static void Main32(string[] args)
        {
            var dimensionSize = 10;
            int[,] matrix = new int[dimensionSize, dimensionSize];
            var random = new Random();
            var maxRandom = 5;
            var numberForSet = 0;
            Console.WriteLine($"Исходная матрица: ");

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] = random.Next(maxRandom + 1);
                    Console.Write($"{matrix[i, j]}\t");
                }

                Console.WriteLine();
            }

            var maxMatrixValue = Int32.MinValue;

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (maxMatrixValue < matrix[i, j])
                    {
                        maxMatrixValue = matrix[i, j];
                    }
                }
            }

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (maxMatrixValue == matrix[i, j])
                    {
                        matrix[i, j] = numberForSet;
                    }
                }
            }

            Console.WriteLine($"Максимальный элемент = {maxMatrixValue}");
            Console.WriteLine($"Измененная матрица: ");

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write($"{matrix[i, j]}\t");
                }

                Console.WriteLine();
            }
        }
    }
}