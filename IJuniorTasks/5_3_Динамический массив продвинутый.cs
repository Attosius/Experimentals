using System;
using System.Collections.Generic;
using System.Linq;

namespace IJuniorTasks
{
    internal class Program53
    {
        static void Main53(string[] args)
        {
            const string CommandSum = "sum";
            const string CommandExit = "exit";

            bool isWork = true;
            var userInputNumbers = new List<int>();

            while (isWork)
            {
                Console.WriteLine($"Текущий массив:");
                foreach (var item in userInputNumbers)
                {
                    Console.Write($"{item} ");
                }

                if (userInputNumbers.Count == 0)
                {
                    Console.Write($"Пустой массив");
                }

                Console.WriteLine($"\n\nВведите команду:");
                Console.WriteLine($"{CommandSum}. Вывести сумму");
                Console.WriteLine($"{CommandExit}. Выход");
                Console.WriteLine($"Любое число - записать число в массив");
                Console.WriteLine();

                var command = Console.ReadLine();

                switch (command)
                {
                    case CommandSum:
                        DoSum(userInputNumbers);
                        break;

                    case CommandExit:
                        isWork = false;
                        break;

                    default:
                        AddNumber(command, userInputNumbers);
                        break;
                }
            }
        }

        private static void DoSum(List<int> userInputNumbers)
        {
            var sum = userInputNumbers.Sum();
            Console.WriteLine($"Сумма введенных чисел: {sum}");
        }

        private static void AddNumber(string? command, List<int> userInputNumbers)
        {
            if (int.TryParse(command, out var userInputNumber))
            {
                userInputNumbers.Add(userInputNumber);
            }
            else
            {
                Console.WriteLine($"Некорректное число");
            }
        }

    }
}