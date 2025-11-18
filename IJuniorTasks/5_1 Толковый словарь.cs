using System;
using System.Collections.Generic;

namespace IJuniorTasks
{
    internal class Program51
    {
        static void Main51(string[] args)
        {
            var explanatoryDictionary = FillDictionary();
            Console.WriteLine("Введите слово для поиска:");
            var userInput = Console.ReadLine();

            if (explanatoryDictionary.TryGetValue(userInput.Trim().ToUpper(), out var description))
            {
                Console.WriteLine($"Значение слова {userInput}: {description}");
            }
            else
            {
                Console.WriteLine($"Значение слова {userInput} не найдено");
            }
        }

        private static Dictionary<string, string> FillDictionary()
        {
            var explanatoryDictionary = new Dictionary<string, string>
            {
                ["ВЕТОШЬ"] = "ветхая одежда, ветхие вещи; отходы текстильного производства",
                ["ЙЁВИК"] = "город и коммуна в Норвегии",
                ["ЩЁГОЛЬ"] = "тот, кто нарядно, изысканно одевается, кто любит наряжаться; франт"
            };

            return explanatoryDictionary;
        }
    }
}