using System;

namespace IJuniorTasks
{
    internal class Program43
    {
        static void Main43(string[] args)
        {
            var arrayToSort = new[] { 1, 2, 3, 4, 5 };
            WriteArray(arrayToSort);

            Shuffle(arrayToSort);

            Console.WriteLine();
            WriteArray(arrayToSort);

            Console.ReadKey();
        }

        public static void Shuffle(int[] array)
        {
            var random = new Random();
            for (int i = 0; i < array.Length; i++)
            {
                var indexToChange = random.Next(array.Length);
                var temp = array[i];
                array[i] = array[indexToChange];
                array[indexToChange] = temp;
            }
        }

        public static void WriteArray(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write($"{array[i]} ");
            }
        }
    }
}