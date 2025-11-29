using System;
using System.Collections.Generic;

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
            var usersPositions = new Dictionary<string, List<string>>();

            usersPositions["Прогер"] = new List<string>()
            {
                "Петров Антон Игоревич",
                "Смирнов Павел Андреевич",
                "Копылов"
            };
            usersPositions["Юзер"] = new List<string>()
            {
                "Папин Игорь Игоревич",
                "Дарья Андреевич"
            };

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
                        AddRecord(usersPositions);
                        break;

                    case CommandShowAllRecords:
                        ShowRecords(usersPositions);
                        break;

                    case CommandRemoveRecord:
                        RemoveRecord(usersPositions);
                        break;

                    case CommandFindRecords:
                        FindRecordsBySurname(usersPositions);
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

        private static void AddRecord(Dictionary<string, List<string>> usersPositions)
        {
            Console.WriteLine($"\n\nВведите Фамилию Имя и Отчество:");
            var userFullName = Console.ReadLine();

            Console.WriteLine($"Введите должность:");
            var userPosition = Console.ReadLine();

            if (!usersPositions.ContainsKey(userPosition))
            {
                usersPositions[userPosition] = new List<string>();
            }

            usersPositions[userPosition].Add(userFullName);
            Console.WriteLine($"Досье с ФИО {userFullName} и должностью {userPosition} успешно добавлено");
        }

        private static void ShowRecords(Dictionary<string, List<string>> usersPositions)
        {
            int positionIndex = 0;
            int userIndex = 0;

            foreach (var usersPositionsKey in usersPositions.Keys)
            {
                positionIndex++;
                Console.WriteLine($"{positionIndex}. Должность {usersPositionsKey}.");
                
                foreach (var user in usersPositions[usersPositionsKey])
                {
                    userIndex++;
                    Console.WriteLine($"\t{userIndex}. {user}.");
                }
            }
        }

        private static void RemoveRecord(Dictionary<string, List<string>> usersPositions)
        {
            ShowRecords(usersPositions);

            Console.WriteLine($"Введите индекс сотрудника для удаления:");
            var indexToRemoveString = Console.ReadLine();

            if (int.TryParse(indexToRemoveString, out var indexToRemove) == false)
            {
                Console.WriteLine($"Некорректный индекс для удаления!");
                return;
            }

            int userIndex = 0;
            string userPosition = string.Empty;
            string userNameToRemove = string.Empty;

            foreach (var usersPositionsKey in usersPositions.Keys)
            {
                foreach (var user in usersPositions[usersPositionsKey])
                {
                    userIndex++;

                    if (userIndex == indexToRemove)
                    {
                        userNameToRemove = user;
                        userPosition = usersPositionsKey;
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(userNameToRemove))
            {
                usersPositions[userPosition].Remove(userNameToRemove);
                Console.WriteLine($"Досье с ФИО {userNameToRemove} успешно удалено");

                if (usersPositions[userPosition].Count == 0)
                {
                    usersPositions.Remove(userPosition);
                }
            }
            else
            {
                Console.WriteLine($"Досье с индексом {indexToRemove} не найдено");
            }
        }
        
        private static void FindRecordsBySurname(Dictionary<string, List<string>> usersPositions)
        {
            Console.WriteLine($"Введите фамилию для поиска:");
            var surnameToFind = Console.ReadLine();
            bool isFind = false;
            var userIndex = 0;

            foreach (var usersPositionsKey in usersPositions.Keys)
            {
                foreach (var user in usersPositions[usersPositionsKey])
                {
                    var surname = user.Split(' ')[0];

                    if (surname.Contains(surnameToFind, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine($"{++userIndex}. ФИО: {user}. Должность: {usersPositionsKey}");
                        isFind = true;
                    }
                }
            }

            if (isFind == false)
            {
                Console.WriteLine($"Досье с фамилией {surnameToFind} не найдено");
            }
        }
    }
}