using System;

namespace IJuniorTasks
{
    internal class Program42
    {
        static void Main42(string[] args)
        {
            DrawBar("Health", 10, 1, 10, '#', ConsoleColor.Red, 40);
            DrawBar("Mana", 10, 2, 10, '#', ConsoleColor.Blue, 80);
            Console.ReadKey();
        }

        public static void DrawBar(string nameBar, int positionX, int positionY, int width, char fillingChar, ConsoleColor fillingColor, int percentFilling)
        {
            Console.SetCursorPosition(positionX, positionY);
            Console.Write($"{nameBar.PadRight(10)}");
            Console.Write('[');

            var countFilling = width * percentFilling / 100;

            var defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = fillingColor;

            for (int i = 0; i < width; i++)
            {
                if (i < countFilling)
                {
                    Console.Write(fillingChar);
                }
                else
                {
                    Console.Write(' ');
                }
            }

            Console.ForegroundColor = defaultColor;
            Console.Write(']');
        }
    }
}