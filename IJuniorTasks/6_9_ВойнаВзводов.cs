using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace IJuniorTasks69
{
    public interface IPullFighterSquad
    {
        Fighter GetRandomAliveFighter();
        bool TryGetRandomAliveFighterExcept(HashSet<Fighter> exceptFighters, out Fighter fighterTarget);
        bool IsAlive { get; }
    }

    internal class Program
    {
        static void Main69(string[] args)
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

                if (Enum.TryParse<Commands>(command, out var commandEnum) == false)
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
        public void CreateFight()
        {
            var firstSquad = new Squad("First");
            firstSquad.Create();
            firstSquad.ShowFighters();
            Console.WriteLine();

            var secondSquad = new Squad("Second");
            secondSquad.Create();
            secondSquad.ShowFighters();
            Console.ReadLine();

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

    public class Squad : IPullFighterSquad
    {
        private const int FighterCounts = 10;

        private List<Fighter> _fighters = new();
        private readonly FightersFactory _fightersFactory = new();

        public Squad(string name)
        {
            Name = name;
        }

        public string Name { get; }
        public bool IsAlive => _fighters.Count(fighter => fighter.IsAlive) > 0;

        public void Create()
        {
            for (int i = 0; i < FighterCounts; i++)
            {
                var fighter = _fightersFactory.GetRandomFighter(Name);
                _fighters.Add(fighter);
            }
        }

        public void Attack(IPullFighterSquad secondSquad)
        {
            for (int i = 0; i < _fighters.Count; i++)
            {
                if (secondSquad.IsAlive == false)
                {
                    return;
                }

                var fighter = _fighters[i];
                fighter.Attack(secondSquad);
            }
        }

        public Fighter GetRandomAliveFighter()
        {
            var aliveFighters = _fighters
                .Where(item => item.IsAlive)
                .ToList();
            var randomIndex = UserUtils.GenerateRandomNumber(aliveFighters.Count - 1);
            return aliveFighters[randomIndex];
        }

        public bool TryGetRandomAliveFighterExcept(HashSet<Fighter> exceptFighters, out Fighter fighterTarget)
        {
            fighterTarget = null;
            var aliveExceptFighters = _fighters
                .Where(item => item.IsAlive)
                .Where(item => exceptFighters.Contains(item) == false)
                .ToList();

            if (aliveExceptFighters.Count == 0)
            {
                return false;
            }

            var randomIndex = UserUtils.GenerateRandomNumber(aliveExceptFighters.Count - 1);
            fighterTarget = aliveExceptFighters[randomIndex];
            return true;
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

        public void ShowFighters()
        {
            Console.WriteLine($"Взвод {Name}:");

            for (int i = 0; i < _fighters.Count; i++)
            {
                Console.Write($"{i + 1:00}. Боец ");
                _fighters[i].ShowInfo();
                Console.WriteLine();
            }
        }
    }

    public class Fighter
    {
        private static int s_fightersCount;

        public Fighter(string squadName, int minDamage, int maxDamage, int armor, int health)
        {
            Name = $"Fighter #{++s_fightersCount} <{squadName}>";
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            Armor = armor;
            Health = health;
        }

        public string Name { get; }
        public int Health { get; protected set; }
        public int MinDamage { get; }
        public int MaxDamage { get; }
        public int Armor { get; }

        public bool IsAlive => Health > 0;
        public bool IsDead => Health <= 0;

        public virtual void ShowInfo()
        {
            UserUtils.Write($"{Name}, простой рубака", ConsoleColor.White);
        }

        public virtual void Attack(IPullFighterSquad targetSquad)
        {
            var damage = UserUtils.GenerateRandomNumber(MinDamage, MaxDamage);
            Console.Write($"\nУдар! {Name}: Бьет! Урон: ");
            UserUtils.Write(damage.ToString(), ConsoleColor.Red);
            Console.WriteLine();

            var fighterTarget = targetSquad.GetRandomAliveFighter();
            fighterTarget.TakeDamage(damage);
        }

        public virtual void TakeDamage(int damage)
        {
            var finishDamage = Math.Max(damage - Armor, 0);
            Console.Write($"Урон! {Name}: Броня: {Armor}. Итоговый урон: {finishDamage}. ");
            Health -= finishDamage;

            Console.Write($"Остаток жизни: ");
            UserUtils.WriteLine($"{Health}", ConsoleColor.Green);

            if (Health < 0)
            {
                UserUtils.WriteLine($"Боец {Name} погиб", ConsoleColor.Red);
            }
        }

        public virtual Fighter Clone(string squadName)
        {
            return new Fighter(squadName, MinDamage, MaxDamage, Armor, Health);
        }
    }

    public class FighterDoubleDamage : Fighter
    {
        private readonly int _damageMultiplier;

        public FighterDoubleDamage(string name, int minDamage, int maxDamage, int armor, int health, int damageMultiplier)
            : base(name, minDamage, maxDamage, armor, health)
        {
            _damageMultiplier = damageMultiplier;
        }

        public override void ShowInfo()
        {
            UserUtils.Write($"{Name}, урон x{_damageMultiplier}", ConsoleColor.DarkYellow);
        }

        public override void Attack(IPullFighterSquad targetSquad)
        {
            var fighterTarget = targetSquad.GetRandomAliveFighter();

            var damage = UserUtils.GenerateRandomNumber(MinDamage, MaxDamage);
            damage *= _damageMultiplier;
            UserUtils.WriteLine($"\nУдар! {Name}: Урон x{_damageMultiplier}: {damage}", ConsoleColor.DarkRed);

            fighterTarget.TakeDamage(damage);
        }

        public override Fighter Clone(string squadName)
        {
            return new FighterDoubleDamage(squadName, MinDamage, MaxDamage, Armor, Health, _damageMultiplier);
        }
    }

    public class FighterMultipleDifferentHit : Fighter
    {
        private readonly int _attacksCountPerRound;

        public FighterMultipleDifferentHit(string name, int minDamage, int maxDamage, int armor, int health, int attacksCountPerRound)
            : base(name, minDamage, maxDamage, armor, health)
        {
            _attacksCountPerRound = attacksCountPerRound;
        }

        public override void ShowInfo()
        {
            UserUtils.Write($"{Name}, множественная атака всегда разных целей", ConsoleColor.DarkRed);
        }

        public override void Attack(IPullFighterSquad targetSquad)
        {
            var fightersTargetsAlreadyHit = new HashSet<Fighter>();

            for (int i = 0; i < _attacksCountPerRound; i++)
            {
                var attackCountText = $"Множественная атака #{i + 1}";

                if (targetSquad.TryGetRandomAliveFighterExcept(fightersTargetsAlreadyHit, out var fighterTarget) == false)
                {
                    UserUtils.WriteLine($"\nПопытка удара! {Name}: {attackCountText}: нет цели для удара!", ConsoleColor.Cyan);
                    return;
                }

                var damage = UserUtils.GenerateRandomNumber(MinDamage, MaxDamage);
                UserUtils.WriteLine($"\nУдар! {Name}: {attackCountText}: {damage}", ConsoleColor.DarkRed);

                fighterTarget.TakeDamage(damage);
                fightersTargetsAlreadyHit.Add(fighterTarget);
            }
        }

        public override Fighter Clone(string squadName)
        {
            return new FighterMultipleDifferentHit(squadName, MinDamage, MaxDamage, Armor, Health, _attacksCountPerRound);
        }
    }

    public class FighterMultipleHit : Fighter
    {
        private readonly int _attacksCountPerRound;

        public FighterMultipleHit(string name, int minDamage, int maxDamage, int armor, int health, int attacksCountPerRound)
            : base(name, minDamage, maxDamage, armor, health)
        {
            _attacksCountPerRound = attacksCountPerRound;
        }

        public override void ShowInfo()
        {
            UserUtils.Write($"{Name}, множественная атака в т.ч. одной цели", ConsoleColor.DarkCyan);
        }

        public override void Attack(IPullFighterSquad targetSquad)
        {
            for (int i = 0; i < _attacksCountPerRound; i++)
            {
                if (targetSquad.IsAlive == false)
                {
                    return;
                }

                var fighterTarget = targetSquad.GetRandomAliveFighter();

                var damage = UserUtils.GenerateRandomNumber(MinDamage, MaxDamage);
                UserUtils.WriteLine($"\nУдар! {Name}: Множественная атака #{i + 1}: {damage}", ConsoleColor.DarkRed);

                fighterTarget.TakeDamage(damage);
            }
        }

        public override Fighter Clone(string squadName)
        {
            return new FighterMultipleHit(squadName, MinDamage, MaxDamage, Armor, Health, _attacksCountPerRound);
        }
    }

    public class FightersFactory
    {
        private readonly List<Fighter> _availableFighters = new List<Fighter>();

        public FightersFactory()
        {
            _availableFighters.Add(new Fighter(default, minDamage: 15, maxDamage: 20, armor: 10, health: 100));
            _availableFighters.Add(new FighterDoubleDamage(default, minDamage: 15, maxDamage: 20, armor: 10, health: 100, damageMultiplier: 2));
            _availableFighters.Add(new FighterMultipleDifferentHit(default, minDamage: 15, maxDamage: 20, armor: 10, health: 100, attacksCountPerRound: 2));
            _availableFighters.Add(new FighterMultipleHit(default, minDamage: 15, maxDamage: 20, armor: 10, health: 100, attacksCountPerRound: 2));
        }

        public Fighter GetRandomFighter(string squadName)
        {
            var fighterTypeIndex = UserUtils.GenerateRandomNumber(_availableFighters.Count - 1);
            var fighterPrefab = _availableFighters[fighterTypeIndex];
            return fighterPrefab.Clone(squadName);
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
