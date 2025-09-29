using System;

namespace IJuniorTasks
{
    internal class Program41
    {
        static void Main41(string[] args)
        {
            var number = ReadInt();
            Console.WriteLine(number);
        }

        public static int ReadInt()
        {
            string numberString;
            int number;

            do
            {
                Console.WriteLine($"Введите число:");
                numberString = Console.ReadLine();
            } while (!int.TryParse(numberString, out number));

            return number;
        }
    }
}