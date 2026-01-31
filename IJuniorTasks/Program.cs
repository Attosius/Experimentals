using System;
using System.Collections.Generic;

namespace IJuniorTasks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var administrator = new Administrator();
            administrator.Run();
        }
    }

    public enum Commands
    {
        AddDay = 1,
        AddFish,
        GetFish,
        Exit,
    }

    public class Administrator
    {
        public void Run()
        {
            bool isWork = true;
            var aquarium = new Aquarium();

            while (isWork)
            {
                Console.Clear();
                aquarium.PaintStats();

                Console.WriteLine($"\n\nВведите команду:");
                Console.WriteLine($"{(int)Commands.AddDay}. Прожить день");
                Console.WriteLine($"{(int)Commands.AddFish}. Добавить рыбку");
                Console.WriteLine($"{(int)Commands.GetFish}. Достать рыбку");
                Console.WriteLine($"{(int)Commands.Exit}. Выход");
                Console.WriteLine();

                var command = Console.ReadLine();

                if (Enum.TryParse<Commands>(command, out var commandEnum) == false)
                {
                    Console.WriteLine($"Некорректная команда!");
                    continue;
                }

                switch (commandEnum)
                {

                    case Commands.AddDay:
                        aquarium.AddDay();
                        break;

                    case Commands.AddFish:
                        aquarium.AddFish();
                        break;

                    case Commands.GetFish:
                        aquarium.GetFish();
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

    public class Aquarium
    {
        public const int MaxFish = 10;

        private readonly List<Fish> _fish = new();
        private readonly FishFactory _fishFactory;

        public Aquarium()
        {
            _fishFactory = new FishFactory();
        }

        public int AquariumAge { get; private set; }

        public void AddDay()
        {
            AquariumAge++;
            var deadFishes = new List<Fish>();

            for (int i = 0; i < _fish.Count; i++)
            {
                var fish = _fish[i];
                fish.AddDay();

                if (fish.IsAlive == false)
                {
                    deadFishes.Add(fish);
                }
            }


            if (deadFishes.Count > 0)
            {
                foreach (var deadFish in deadFishes)
                {
                    _fish.Remove(deadFish);
                    Console.Write($"Рыбка ");
                    UserUtils.Write($"{deadFish.Name}", ConsoleColor.Cyan);
                    Console.WriteLine($" умерла :(");
                }

                Console.ReadKey();
            }
        }

        public void AddFish()
        {
            if (_fish.Count >= MaxFish)
            {
                Console.Write($"Аквариум уже заполнен полностью!");
                Console.ReadKey();
                return;
            }

            var fish = _fishFactory.CreateRandomFish();
            _fish.Add(fish);

            Console.Write($"В аквариум добавили ");
            UserUtils.Write($"{fish.Name}", ConsoleColor.Cyan);
            Console.WriteLine($", ee максимальный возраст: {fish.MaxLife}");
            Console.ReadKey();
        }

        public void PaintStats()
        {
            Console.WriteLine($"Возраст аквариума {AquariumAge}");
            Console.Write($"Сейчас в аквариуме ");

            if (_fish.Count == 0)
            {
                UserUtils.Write($"пусто :(", ConsoleColor.Yellow);
                return;
            }

            Console.Write($"рыбок: ");
            UserUtils.WriteLine($"{_fish.Count}.", ConsoleColor.Cyan);

            for (int i = 0; i < _fish.Count; i++)
            {
                var fish = _fish[i];
                Console.Write($"{i + 1}. Рыбка ");
                UserUtils.Write($"{fish.Name}", ConsoleColor.Cyan);
                Console.WriteLine($", ee возраст: {fish.CurrentLife} из {fish.MaxLife}");
            }

        }

        public void GetFish()
        {
            PaintStats();

            Console.WriteLine($"Введите номер рыбки, которую хотите достать:");
            var indexText = Console.ReadLine();

            if (int.TryParse(indexText, out var index) == false || index > _fish.Count || index < 1)
            {
                Console.WriteLine($"Некорректный индекс!");
                Console.ReadKey();
                return;
            }

            var fish = _fish[index - 1];
            _fish.Remove(fish);

            Console.Write($"Рыбку ");
            UserUtils.Write($"{fish.Name}", ConsoleColor.Cyan);
            Console.WriteLine($" достали из аквариума");
            Console.ReadKey();
        }
    }

    public class Fish
    {
        public Fish(string name, int maxLife)
        {
            Name = name;
            MaxLife = maxLife;
        }

        public string Name { get; }
        public int MaxLife { get; }
        public int CurrentLife { get; private set; }
        public bool IsAlive => CurrentLife < MaxLife;

        public void AddDay()
        {
            CurrentLife++;
        }

        public Fish Clone()
        {
            return new Fish(Name, MaxLife);
        }
    }

    public class FishFactory
    {
        private readonly List<Fish> _availableFish = new();

        public FishFactory()
        {
            _availableFish.Add(new Fish("Обычная", maxLife: 5));
            _availableFish.Add(new Fish("Петушок", maxLife: 3));
            _availableFish.Add(new Fish("Сомик-прилипала", maxLife: 15));
            _availableFish.Add(new Fish("Золотая рыбка", maxLife: 10));
        }

        public Fish CreateRandomFish()
        {
            var index = UserUtils.GenerateRandomNumber(_availableFish.Count - 1);
            var fish = _availableFish[index].Clone();
            return fish;
        }
    }

    public class UserUtils
    {
        private static readonly Random s_random = new();

        public static int GenerateRandomNumber(int max)
        {
            return s_random.Next(max + 1);
        }

        public static int GenerateRandomNumber(int min, int max)
        {
            return s_random.Next(min, max + 1);
        }

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
