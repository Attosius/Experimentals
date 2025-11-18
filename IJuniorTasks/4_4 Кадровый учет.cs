using System;

namespace IJuniorTasks
{
    internal class Program44
    {
        static void Main44(string[] args)
        {
            const string CommandAddRecord = "1";
            const string CommandShowAllRecords = "2";
            const string CommandRemoveRecord = "3";
            const string CommandFindRecords = "4";
            const string CommandExit = "5";

            bool isWork = true;
            string[] usersFullNames = new string[0];
            string[] usersPositions = new string[0];

            while (isWork)
            {
                Console.WriteLine($"\n\nВведите команду:");
                Console.WriteLine($"{CommandAddRecord}. Добавить досье");
                Console.WriteLine($"{CommandShowAllRecords}. Показать все досье");
                Console.WriteLine($"{CommandRemoveRecord}. Удалить досье");
                Console.WriteLine($"{CommandFindRecords}. Найти досье");
                Console.WriteLine($"{CommandExit}. Выход");
                Console.WriteLine();

                var command = Console.ReadLine();

                switch (command)
                {
                    case CommandAddRecord:
                        AddRecord(ref usersFullNames, ref usersPositions);
                        break;

                    case CommandShowAllRecords:
                        ShowRecords(usersFullNames, usersPositions);
                        break;

                    case CommandRemoveRecord:
                        RemoveRecord(ref usersFullNames, ref usersPositions);
                        break;

                    case CommandFindRecords:
                        FindRecordsBySurname(usersFullNames, usersPositions);
                        break;

                    case CommandExit:
                        isWork = false;
                        break;

                    default:
                        Console.WriteLine($"Некорректная команда!");
                        break;
                }
            }
        }

        private static void AddRecord(ref string[] usersFullNames, ref string[] usersPositions)
        {
            Console.WriteLine($"\n\nВведите Фамилию Имя и Отчество:");
            var userFullName = Console.ReadLine();
            ExpandArray(usersFullNames, userFullName);

            Console.WriteLine($"Введите должность:");
            var userPosition = Console.ReadLine();
            ExpandArray(usersPositions, userPosition);

            Console.WriteLine($"Досье с ФИО {userFullName} и должностью {userPosition} успешно добавлено");
        }

        private static void ShowRecords(string[] usersFullNames, string[] usersPositions)
        {
            for (int i = 0; i < usersFullNames.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {usersFullNames[i]} - {usersPositions[i]}");
            }
        }

        private static void RemoveRecord(ref string[] usersFullNames, ref string[] usersPositions)
        {
            ShowRecords(usersFullNames, usersPositions);

            Console.WriteLine($"Введите индекс для удаления:");
            var indexToRemoveString = Console.ReadLine();

            if (int.TryParse(indexToRemoveString, out var indexToRemove) == false)
            {
                Console.WriteLine($"Некорректный индекс для удаления!");
                return;
            }

            if (indexToRemove < 1 || indexToRemove > usersFullNames.Length)
            {
                Console.WriteLine($"Некорректный индекс для удаления!");
                return;
            }

            usersFullNames = TrimArray(usersFullNames, indexToRemove);
            usersPositions = TrimArray(usersPositions, indexToRemove);
        }

        private static string[] ExpandArray(string[] array, string itemToAdd)
        {
            var tempArray = new string[array.Length + 1];

            for (int i = 0; i < array.Length; i++)
            {
                tempArray[i] = array[i];
            }

            tempArray[array.Length - 1] = itemToAdd;
            return tempArray;
        }

        private static string[] TrimArray(string[] array, int indexToRemove)
        {
            var tempArray = new string[array.Length - 1];
            var newArrayIndex = 0;

            for (int i = 0; i < array.Length; i++)
            {
                if (i + 1 == indexToRemove)
                {
                    continue;
                }

                tempArray[newArrayIndex++] = array[i];
            }

            return tempArray;
        }

        private static void FindRecordsBySurname(string[] usersFullNames, string[] usersPositions)
        {
            Console.WriteLine($"Введите фамилию для поиска:");
            var surnameToFind = Console.ReadLine();
            bool isFind = false;

            for (int i = 0; i < usersFullNames.Length; i++)
            {
                var surname = usersFullNames[i].Split(' ')[0];
                if (surname == surnameToFind)
                {
                    Console.WriteLine($"{i + 1}. {usersFullNames[i]} - {usersPositions[i]}");
                    isFind = true;
                }
            }

            if (isFind == false)
            {
                Console.WriteLine($"Досье с фамилией {surnameToFind} не найдено");
            }
        }
    }
}