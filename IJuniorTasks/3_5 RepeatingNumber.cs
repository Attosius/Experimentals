using System;

namespace IJuniorTasks
{
    internal class Program35
    {
        static void Main35(string[] args)
        {
            var arraySize = 30;
            int[] numbers = new int[arraySize];

            var random = new Random();
            var maxRandom = 2;

            Console.WriteLine($"Текущий массив:");

            for (int i = 0; i < numbers.Length; i++)
            {
                numbers[i] = random.Next(maxRandom + 1);
                Console.Write($"{numbers[i]} ");
            }

            var numberRepeatsItself = numbers[0];
            var repeatingCount = 1;

            var numberRepeatsItselfTemp = numberRepeatsItself;
            var repeatingCountTemp = repeatingCount;

            for (int i = 1; i < numbers.Length; i++)
            {
                if (numbers[i] == numberRepeatsItselfTemp)
                {
                    repeatingCountTemp++;

                    if (repeatingCountTemp > repeatingCount)
                    {
                        numberRepeatsItself = numberRepeatsItselfTemp;
                        repeatingCount = repeatingCountTemp;
                    }
                }
                else
                {
                    numberRepeatsItselfTemp = numbers[i];
                    repeatingCountTemp = 1;
                }
            }

            Console.WriteLine($"\nЧисло {numberRepeatsItself} повторяется наибольшее количество раз: {repeatingCount}");
        }
    }
}