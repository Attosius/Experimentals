using System;
using System.Collections.Generic;

namespace IJuniorTasks
{
    internal class Очередьвмагазине
    {
        static void MainОчередьвмагазине(string[] args)
        {
            var purchasesSums = new Queue<int>();
            purchasesSums.Enqueue(1);
            purchasesSums.Enqueue(10);
            purchasesSums.Enqueue(7);
            purchasesSums.Enqueue(3);

            int accountSum = 0;

            while (purchasesSums.TryDequeue(out var purchaseSum))
            {
                accountSum += purchaseSum;
                Console.WriteLine($"Обслужен клиент с суммой {purchaseSum}. Общая сумма аккаунта {accountSum}");
                Console.WriteLine($"\n\nНажмите любую клавишу для продолжения");
                Console.ReadLine();
                Console.Clear();
            }

            Console.WriteLine($"Обслужены все клиенты. Общая сумма аккаунта {accountSum}");
            Console.ReadLine();
        }
    }
}