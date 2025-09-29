using System;

namespace IJuniorTasks
{
    internal class Program31
    {
        static void Main31(string[] args)
        {
            int[,] array =
            {
                { 1, 11, 111 },
                { 2, 22, 222 },
                { 3, 33, 333 },
                { 4, 44, 444 }
            };
            var rowToSum = 2;
            var colToMul = 1;

            var sum = 0;
            var mul = 1;

            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    Console.Write($"{array[i, j]} ");
                }
                Console.WriteLine();
            }

            for (int i = 0; i < array.GetLength(1); i++)
            {
                sum += array[rowToSum - 1, i];
            }

            for (int i = 0; i < array.GetLength(0); i++)
            {
                mul *= array[i, colToMul - 1];
            }

            Console.WriteLine($"Сумма второй строки {sum}");
            Console.WriteLine($"Произведение первого столбца {mul}");
        }
    }
}