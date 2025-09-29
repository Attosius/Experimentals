using System;

namespace IJuniorTasks
{
    internal class Program7
    {
        static void Main7(string[] args)
        {
            Console.WriteLine("Введите имя ");
            var userName = Console.ReadLine();
            Console.WriteLine("Введите символ ");
            var symbol = Console.ReadLine();

            for (int i = 0; i < userName.Length + 2; i++)
            {
                Console.Write(symbol);
            }

            Console.Write($"\n{symbol}");
            Console.Write($"{userName}");
            Console.Write($"{symbol}\n");

            for (int i = 0; i < userName.Length + 2; i++)
            {
                Console.Write(symbol);
            }
        }
    }
}