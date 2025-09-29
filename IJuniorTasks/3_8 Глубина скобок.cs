using System;

namespace IJuniorTasks
{
    internal class Program38
    {
        static void Main38(string[] args)
        {
            var bracesString = "(()(()))";
            var d = false;
            var s = sizeof(bool);
            Console.WriteLine($"Текущая строка: {s}");

            var bracesDiff = 0;
            var maxBracesDepth = 0;
            var openBrace = '(';

            for (int i = 0; i < bracesString.Length; i++)
            {
                if (bracesString[i] == openBrace)
                {
                    bracesDiff++;

                    if (bracesDiff > maxBracesDepth)
                    {
                        maxBracesDepth = bracesDiff;
                    }
                }
                else
                {
                    bracesDiff--;
                    if (bracesDiff < 0)
                    {
                        break;
                    }
                }
            }

            if (bracesDiff == 0)
            {
                Console.WriteLine($"Строка корректна, максимальная глубина: {maxBracesDepth}");
            }
            else
            {
                Console.WriteLine($"Строка некорректна, максимальная глубина: {maxBracesDepth}");
            }
        }

    }
}