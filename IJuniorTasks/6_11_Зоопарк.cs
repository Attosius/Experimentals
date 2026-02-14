using System;
using System.Collections.Generic;

namespace IJuniorTasks611
{
    internal class Program
    {
        public static void Main611(string[] args)
        {
            var administrator = new Administrator();
            administrator.Run();
        }
    }

    public enum Commands
    {
        VisitEnclosure = 1,
        Exit,
    }

    public class Administrator
    {
        public void Run()
        {
            bool isWork = true;
            var zoo = new Zoo();

            while (isWork)
            {
                Console.Clear();
                zoo.ShowMenu();

                Console.WriteLine($"\n\nВведите команду:");
                Console.WriteLine($"{(int)Commands.VisitEnclosure}. Подойти к вольеру");
                Console.WriteLine($"{(int)Commands.Exit}. Выход");
                Console.WriteLine();

                var command = Console.ReadLine();

                if (Enum.TryParse<Commands>(command, out var commandEnum) == false)
                {
                    Console.WriteLine($"Некорректная команда!");
                    Console.ReadKey();
                    continue;
                }

                switch (commandEnum)
                {
                    case Commands.VisitEnclosure:
                        zoo.VisitEnclosure();
                        break;

                    case Commands.Exit:
                        isWork = false;
                        break;

                    default:
                        Console.WriteLine($"Некорректная команда!");
                        break;
                }
            }
        }
    }

    public class Zoo
    {
        private readonly List<Enclosure> _enclosures = new();
        private readonly EnclosureFactory _enclosureFactory;

        public Zoo()
        {
            _enclosureFactory = new EnclosureFactory();
            InitializeEnclosures();
        }

        private void InitializeEnclosures()
        {
            _enclosures.Add(_enclosureFactory.CreateLionEnclosure());
            _enclosures.Add(_enclosureFactory.CreateElephantEnclosure());
            _enclosures.Add(_enclosureFactory.CreateMonkeyEnclosure());
            _enclosures.Add(_enclosureFactory.CreatePenguinEnclosure());
        }

        public void ShowMenu()
        {
            Console.WriteLine("=== ЗООПАРК ===");
            Console.WriteLine("Доступные вольеры:");

            for (int i = 0; i < _enclosures.Count; i++)
            {
                var enclosure = _enclosures[i];
                Console.Write($"{i + 1}. ");
                UserUtils.Write($"{enclosure.Name}", ConsoleColor.Green);
                Console.WriteLine($" ({enclosure.Animals.Count} животных)");
            }
        }

        public void VisitEnclosure()
        {
            Console.WriteLine($"Введите номер вольера, к которому хотите подойти:");
            var indexText = Console.ReadLine();

            if (int.TryParse(indexText, out var index) == false || index > _enclosures.Count || index < 1)
            {
                Console.WriteLine($"Некорректный номер вольера!");
                Console.ReadKey();
                return;
            }

            var enclosure = _enclosures[index - 1];
            enclosure.ShowInfo();
            Console.ReadKey();
        }
    }

    public class Enclosure
    {
        public Enclosure(string name, List<Animal> animals)
        {
            Name = name;
            Animals = animals;
        }

        public string Name { get; }
        public List<Animal> Animals { get; }

        public void ShowInfo()
        {
            Console.Clear();
            Console.WriteLine($"=== Вольер: {Name} ===");
            Console.WriteLine($"Количество животных: {Animals.Count}");
            Console.WriteLine();

            for (int i = 0; i < Animals.Count; i++)
            {
                var animal = Animals[i];
                Console.Write($"{i + 1}. ");
                UserUtils.Write($"{animal.Name}", ConsoleColor.Cyan);
                Console.Write($", пол: ");
                UserUtils.Write($"{animal.Gender}", ConsoleColor.Yellow);
                Console.Write($", звук: ");
                UserUtils.WriteLine($"\"{animal.Sound}\"", ConsoleColor.Magenta);
            }
        }
    }

    public class Animal
    {
        public Animal(string name, Gender gender, string sound)
        {
            Name = name;
            Gender = gender;
            Sound = sound;
        }

        public string Name { get; }
        public Gender Gender { get; }
        public string Sound { get; }
    }

    public enum Gender
    {
        Самец,
        Самка
    }

    public class EnclosureFactory
    {
        public Enclosure CreateLionEnclosure()
        {
            var animals = new List<Animal>
            {
                new Animal("Лев", Gender.Самец, "Рррр!"),
                new Animal("Львица", Gender.Самка, "Рррр!"),
                new Animal("Левёнок", Gender.Самец, "Мяу!")
            };
            return new Enclosure("Львиный вольер", animals);
        }

        public Enclosure CreateElephantEnclosure()
        {
            var animals = new List<Animal>
            {
                new Animal("Слон", Gender.Самец, "Тууу!"),
                new Animal("Слониха", Gender.Самка, "Тууу!"),
                new Animal("Слонёнок", Gender.Самка, "Пиу!")
            };
            return new Enclosure("Слоновий вольер", animals);
        }

        public Enclosure CreateMonkeyEnclosure()
        {
            var animals = new List<Animal>
            {
                new Animal("Обезьяна", Gender.Самка, "У-у-у-а!"),
                new Animal("Шимпанзе", Gender.Самец, "У-у-у-а!"),
                new Animal("Мартышка", Gender.Самка, "И-и-и!"),
                new Animal("Горилла", Gender.Самец, "Хррр!")
            };
            return new Enclosure("Обезьяний вольер", animals);
        }

        public Enclosure CreatePenguinEnclosure()
        {
            var animals = new List<Animal>
            {
                new Animal("Пингвин", Gender.Самец, "Кря!"),
                new Animal("Пингвиниха", Gender.Самка, "Кря!"),
                new Animal("Пингвинёнок", Gender.Самец, "Пи-пи!")
            };
            return new Enclosure("Пингвиний вольер", animals);
        }
    }

    public class UserUtils
    {
        public static void WriteLine(string data, ConsoleColor color)
        {
            Write(data, color);
            Console.WriteLine();
        }

        public static void Write(string data, ConsoleColor color)
        {
            var initColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(data);
            Console.ForegroundColor = initColor;
        }
    }
}
