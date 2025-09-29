using System;

namespace IJuniorTasks
{
    internal class Program37
    {
        static void Main37(string[] args)
        {
            int[] numbersForShift = { 1, 2, 3, 4 };

            Console.WriteLine($"Текущий массив: ");

            foreach (var number in numbersForShift)
            {
                Console.Write($"{number} ");
            }

            Console.WriteLine($"\n\nВведите количество циклов сдвига: ");
            var shiftCount = Convert.ToInt32(Console.ReadLine());

            for (int j = 0; j < shiftCount; j++)
            {
                var savedNumber = numbersForShift[0];

                for (int i = numbersForShift.Length - 1; i >= 0; i--)
                {
                    var tempNumber = numbersForShift[i];
                    numbersForShift[i] = savedNumber;
                    savedNumber = tempNumber;
                }
            }

            Console.WriteLine($"\n\nМассив после сдвига влево {shiftCount} раз: ");

            foreach (var number in numbersForShift)
            {
                Console.Write($"{number} ");
            }
        }
    }
}