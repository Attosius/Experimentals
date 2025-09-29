using System;

namespace IJuniorTasks
{
    internal class Program36
    {
        static void Main36(string[] args)
        {
            var arraySize = 30;
            int[] numbers = new int[arraySize];

            var random = new Random();
            var maxRandom = 5;

            Console.WriteLine($"Изначальный массив:");

            for (int i = 0; i < numbers.Length; i++)
            {
                numbers[i] = random.Next(maxRandom + 1);
                Console.Write($"{numbers[i]} ");
            }

            for (int i = 1; i < numbers.Length; i++)
            {
                for (int j = 0; j < numbers.Length - i; j++)
                {
                    if (numbers[j] > numbers[j + 1])
                    {
                        var tmp = numbers[j + 1];
                        numbers[j + 1] = numbers[j];
                        numbers[j] = tmp;
                    }
                }
            }

            Console.WriteLine($"\nОтсортированный массив:");

            for (int i = 0; i < numbers.Length; i++)
            {
                Console.Write($"{numbers[i]} ");
            }
        }
    }
}