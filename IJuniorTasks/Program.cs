using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace IJuniorTasks
{
    public interface IDamageable
    {
        void TakeDamage(int damage);
    }

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
        Fight = 1,
        Exit
    }

    public class Administrator
    {
        public void Run()
        {
            bool isWork = true;

            var war = new War();

            while (isWork)
            {
                Console.Clear();

                Console.WriteLine($"\n\nВведите команду:");
                Console.WriteLine($"{(int)Commands.Fight}. Посмотреть бой");
                Console.WriteLine($"{(int)Commands.Exit}. Выход");
                Console.WriteLine();

                var command = Console.ReadLine();

                if (!Enum.TryParse<Commands>(command, out var commandEnum))
                {
                    Console.WriteLine($"Некорректная команда!");
                    continue;
                }

                switch (commandEnum)
                {

                    case Commands.Fight:
                        war.CreateFight();
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

    public class War
    {

        public War()
        {
        }
        
        public void CreateFight()
        {
            var firstSquad = new Squad("First");
            firstSquad.CreateSquad();

            var secondSquad = new Squad("Second");
            secondSquad.CreateSquad();

            var battleField = new BattleField();
            battleField.Fight(firstSquad, secondSquad);
            Console.ReadLine();
        }
        
        
    }

    public class BattleField
    {
        public void Fight(Squad firstSquad, Squad secondSquad)
        {
            UserUtils.WriteLine($"Бой между {firstSquad.Name} и {secondSquad.Name}!", ConsoleColor.Cyan);
            Thread.Sleep(500);

            var round = 0;

            while (firstSquad.IsAlive && secondSquad.IsAlive)
            {
                UserUtils.WriteLine($"\nРаунд {++round}!", ConsoleColor.DarkGray);

                firstSquad.Attack(secondSquad);
                Console.WriteLine();
                secondSquad.Attack(firstSquad);


                Console.WriteLine();
                Console.WriteLine("=====");
                firstSquad.PaintStats();
                secondSquad.PaintStats();
                Console.WriteLine("=====");

                firstSquad.CleanFighters();
                secondSquad.CleanFighters();
                Thread.Sleep(400);
            }

            ShowFightResult(firstSquad, secondSquad);
        }

        private static void ShowFightResult(Squad firstSquad, Squad secondSquad)
        {
            var winner = firstSquad.IsAlive ? firstSquad : secondSquad;
            UserUtils.WriteLine($"\nПобедил {winner.Name}!", ConsoleColor.Cyan);
        }
    }

    public class Squad
    {
        public const int FighterCounts = 10;
        private List<Fighter> _fighters = new List<Fighter>();
        private readonly FightersFactory _fightersFactory = new FightersFactory();


        public Squad(string name)
        {
            Name = name;
        }

        public string Name { get; }
        public bool IsAlive => _fighters.Count(fighter => fighter.IsAlive) > 0;

        public void CreateSquad()
        {
            for (int i = 0; i < FighterCounts; i++)
            {
                var fighter = _fightersFactory.GetFighter(Name);
                _fighters.Add(fighter);
            }
        }

        public void Attack(Squad secondSquad)
        {
            for (int i = 0; i < _fighters.Count; i++)
            {
                if (!secondSquad.IsAlive)
                {
                    return;
                }

                var fighter = _fighters[i];
                
                var fighterTarget = secondSquad.GetRandomAliveFighter();
                fighter.Attack(fighterTarget);
            }
        }

        private Fighter GetRandomAliveFighter()
        {
            var aliveFighters = _fighters
                .Where(item => item.IsAlive)
                .ToList();
            var randomIndex = UserUtils.GenerateRandomNumber(aliveFighters.Count - 1);
            return aliveFighters[randomIndex];
        }

        public void PaintStats()
        {
            var alive = _fighters.Count(item => item.IsAlive);
            PaintStat(ConsoleColor.Green, alive);
            var dead = _fighters.Count(item => item.IsDead);
            Console.WriteLine($"В живых {alive} бойцов, в последней атаке погибло {dead}.");
        }

        public void PaintStat(ConsoleColor color, double width)
        {
            const string FullBlockSymbol = "\u2588";
            const string VerticalSymbol = "\u2502";

            Console.Write($"{$"{Name}:",-7}");
            UserUtils.Write(VerticalSymbol, ConsoleColor.White);

            for (int i = 0; i < FighterCounts; i++)
            {
                if (i < width)
                {
                    UserUtils.Write(FullBlockSymbol, color);
                }
                else
                {
                    UserUtils.Write(FullBlockSymbol, ConsoleColor.Red);
                }
            }

            UserUtils.WriteLine(VerticalSymbol, ConsoleColor.White);
        }

        public void CleanFighters()
        {
            _fighters = _fighters.Where(item => item.IsAlive).ToList();
        }
    }

    public class Fighter : IDamageable
    {
        public const int MaxPercent = 100;

        public static int FightersCount { get; private set; }

        protected int AttackCount;
        protected int MaxHealth;

        public Fighter(string squadName, int minDamage, int maxDamage, int armor, int health)
        {
            Name = $"Fighter #{++FightersCount} <{squadName}>";
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            Armor = armor;
            Health = health;
            MaxHealth = health;
        }

        public string Name { get; }
        public int Health { get; protected set; }
        public int MinDamage { get; }
        public int MaxDamage { get; }
        public int Armor { get; }

        public bool IsAlive => Health > 0;
        public bool IsDead => Health <= 0;
        
        public virtual void TakeDamage(int damage)
        {
            var finishDamage = Math.Max(damage - Armor, 0);
            Console.Write($"Урон! {Name}: Броня: {Armor}. Итоговый урон: {finishDamage}. ");
            Health -= finishDamage;

            Console.Write($"Остаток жизни: ");
            UserUtils.WriteLine($"{Health}", ConsoleColor.Green);

            if (Health < 0)
            {
                UserUtils.WriteLine($"Боец {Name} погиб" , ConsoleColor.Red);
            }
        }

        public virtual void Attack(IDamageable damageable)
        {
            var damage = UserUtils.GenerateRandomNumber(MinDamage, MaxDamage);
            Attack(damageable, damage, true);
        }

        protected void Attack(IDamageable damageable, bool incrementAttackCount)
        {
            var damage = UserUtils.GenerateRandomNumber(MinDamage, MaxDamage);
            Attack(damageable, damage, incrementAttackCount);
        }

        protected void Attack(IDamageable damageable, int damage, bool incrementAttackCount = true)
        {
            Console.Write($"\nУдар! {Name}: Бьет! Урон: ");
            UserUtils.Write(damage.ToString(), ConsoleColor.Red);
            Console.WriteLine();
            damageable.TakeDamage(damage);

            if (incrementAttackCount)
            {
                IncrementAttackCount();
            }
        }

        protected void IncrementAttackCount()
        {
            AttackCount++;
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

    public class FightersFactory
    {
        public Fighter GetFighter(string squadName)
        {
            var fighter = new Fighter(squadName, minDamage: 15, maxDamage: 20, armor: 10, health: 100);
            return fighter;
        }
    }
}