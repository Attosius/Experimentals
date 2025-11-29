using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IJuniorTasks
{
    internal class _3_4_SumExit
    {
        public static void Main34()
        {

            const string CommandSum = "sum";
            const string CommandExit = "exit";

            bool isWork = true;
            int[] userInputNumbers = new int[0];

            while (isWork)
            {
                Console.WriteLine($"Текущий массив:");
                foreach (var item in userInputNumbers)
                {
                    Console.Write($"{item} ");
                }

                if (userInputNumbers.Length == 0)
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
                        var sum = 0;
                        for (int i = 0; i < userInputNumbers.Length; i++)
                        {
                            sum += userInputNumbers[i];
                        }

                        Console.WriteLine($"Сумма введенных чисел: {sum}");
                        break;
                    case CommandExit:
                        isWork = false;
                        break;
                    default:
                        var userInputNumber = Convert.ToInt32(command);
                        var temp = userInputNumbers;
                        userInputNumbers = new int[temp.Length + 1];

                        for (int i = 0; i < temp.Length; i++)
                        {
                            userInputNumbers[i] = temp[i];
                        }

                        userInputNumbers[userInputNumbers.Length - 1] = userInputNumber;
                        break;
                }

            }
        }
    }
}
