using System;

namespace IJuniorTasks
{
    internal class Program
    {
        static void Main(string[] args)
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
            var sizeDeltaAdd = 1;
            usersFullNames = ExpandArray(usersFullNames, sizeDeltaAdd);
            usersPositions = ExpandArray(usersPositions, sizeDeltaAdd);

            Console.WriteLine($"\n\nВведите Фамилию Имя и Отчество:");
            var userFullName = Console.ReadLine();
            usersFullNames[usersFullNames.Length - 1] = userFullName;

            Console.WriteLine($"Введите должность:");
            var userPosition = Console.ReadLine();
            usersPositions[usersPositions.Length - 1] = userPosition;

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
            TrimRecord(ref usersFullNames, ref usersPositions);
        }

        private static string[] ExpandArray(string[] array, int sizeDelta)
        {
            var tempArray = new string[array.Length + sizeDelta];

            for (int i = 0; i < array.Length; i++)
            {
                tempArray[i] = array[i];
            }

            return tempArray;
        }

        private static void TrimRecord(ref string[] usersFullNames, ref string[] usersPositions)
        {
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

            TrimArrays(ref usersFullNames, ref usersPositions, indexToRemove);
        }

        private static void TrimArrays(ref string[] usersFullNames, ref string[] usersPositions, int indexToRemove)
        {
            var tempUsersFullNames = new string[usersFullNames.Length - 1];
            var tempUsersPositions = new string[usersPositions.Length - 1];
            var newArrayIndex = 0;

            for (int i = 0; i < usersFullNames.Length; i++)
            {
                if (i + 1 == indexToRemove)
                {
                    continue;
                }

                tempUsersFullNames[newArrayIndex] = usersFullNames[i];
                tempUsersPositions[newArrayIndex] = usersPositions[i];
                newArrayIndex++;
            }

            usersFullNames = tempUsersFullNames;
            usersPositions = tempUsersPositions;
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