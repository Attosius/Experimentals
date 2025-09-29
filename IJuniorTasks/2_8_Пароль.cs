using System;

namespace IJuniorTasks
{
    internal class Program8
    {
        static void Main8(string[] args)
        {
            var password = "1";
            var secretMessage = "Hi!";

            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine("Введите пароль ");
                var userInput = Console.ReadLine();
                if (userInput == password)
                {
                    Console.WriteLine($"Секретное сообщение: {secretMessage}");
                    break;
                }
                else
                {
                    Console.WriteLine("Пароль неверный!");
                }
            }
        }
    }
}