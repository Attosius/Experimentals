using System;

namespace IJuniorTasks
{
    internal class Program33
    {
        static void Main33(string[] args)
        {
            var arraySize = 30;
            int[] array = new int[arraySize];
            var random = new Random();
            var maxRandom = 10;

            for (var i = 0; i < array.Length; i++)
            {
                array[i] = random.Next(maxRandom);
            }

            Console.WriteLine($"Исходный массив: ");

            foreach (var item in array)
            {
                Console.WriteLine($"{item} ");
            }

            if (array[0] > array[1])
            {
                Console.WriteLine($"Локальный максимум {array[0]}, i = {0}");
            }

            for (var i = 1; i < array.Length - 1; i++)
            {
                if (array[i] > array[i - 1] && array[i] > array[i + 1])
                {
                    Console.WriteLine($"Локальный максимум {array[i]}, i = {i}");
                }
            }

            if (array[array.Length - 1] > array[array.Length - 2])
            {
                Console.WriteLine($"Локальный максимум {array[array.Length - 1]}, i = {array.Length - 1}");
            }

        }
    }
}