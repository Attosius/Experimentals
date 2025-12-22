using IJuniorTasks66;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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

    public class Administrator
    {
        private const string CommandWelcomeMessage = "1";
        private const string CommandFight = "2";
        private const string CommandExit = "3";

        public void Run()
        {
            bool isWork = true;

            var coliseum = new Coliseum();

            while (isWork)
            {
                Console.Clear();
                coliseum.ShowWelcomeMessage();

                Console.WriteLine($"\n\nВведите команду:");
                Console.WriteLine($"{CommandWelcomeMessage}. Приветственное сообщение");
                Console.WriteLine($"{CommandFight}. Посмотреть бой");
                Console.WriteLine($"{CommandExit}. Выход");
                Console.WriteLine();

                var command = Console.ReadLine();

                switch (command)
                {
                    case CommandWelcomeMessage:
                        coliseum.ShowWelcomeMessage();
                        break;

                    case CommandFight:
                        coliseum.CreateFight();
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
    }

    public class Coliseum
    {

        private List<Fighter> _fighters = new();

        public Coliseum()
        {
            _fighters = ArenaDungeons.GetAllFighters();
        }

        public void ShowWelcomeMessage()
        {
            Console.WriteLine("Ave, Caesar, morituri te salutant!");
        }

        public void CreateFight()
        {
            Console.WriteLine("Выберите первого бойца:");

            if (!TryGetFighter(out Fighter firstFighter))
            {
                Console.ReadLine();
                return;
            }

            Console.WriteLine($"Выберите второго бойца:");

            if (!TryGetFighter(out Fighter secondFighter))
            {
                Console.ReadLine();
                return;
            }
            Arena arena = new Arena();
            arena.Fight(firstFighter, secondFighter);
            Console.ReadLine();
        }

        public void ShowFighters()
        {
            var index = 0;

            foreach (var fighter in _fighters)
            {
                Console.WriteLine($"{++index}. {fighter.Name}");    
            }
        }

        private bool TryGetFighter(out Fighter fighter)
        {
            fighter = default;

            while (fighter == default)
            {
                ShowFighters();
                var fighterIndexStr = Console.ReadLine();

                if (!int.TryParse(fighterIndexStr, out var fighterIndex) || fighterIndex < 0 || fighterIndex > _fighters.Count)
                {
                    Console.WriteLine($"Некорректный индекс! Введите число от 1 до {_fighters.Count}");
                    continue;
                }

                var currentFighter = (Fighter)_fighters[fighterIndex - 1].Clone();

                Console.WriteLine($"Вы выбрали {currentFighter.Name}! Его характеристики:");
                currentFighter.ShowInfo();

                Console.WriteLine($"Продолжить с этим бойцом? (y/n)");
                var answer = Console.ReadLine();

                if (answer.Equals("y", StringComparison.OrdinalIgnoreCase))
                {
                    fighter = currentFighter;
                    return true;
                }

                Console.WriteLine($"Выберите бойца:");
            }

            return true;
        }
    }
    public class FighterDualAttack : Fighter
    {
        public FighterDualAttack(string name, int damage) : base(name, damage)
        {
        }

        public override void ShowInfo()
        {
            base.ShowInfo();
            Console.WriteLine($"Способность: двойной урон с вероятностью {DualAttackPercent}%");
        }

        public int DualAttackPercent { get; } = 30;

        public override void Attack(IDamageable damageable)
        {
            var damage = Damage * 2;

            damageable.TakeDamage(damage);
            Console.WriteLine($"{Name} бьет! Двойной удар! Урон: {damage}");
        }
        public override object Clone()
        {
            return new FighterDualAttack(Name, Damage);
        }
    }

    public class Fighter : ICloneable, IDamageable
    {
        public Fighter(string name, int damage)
        {
            Name = name;
            Damage = damage;
        }
        public string Name { get; } = "Fedor";
        public int Damage { get; private set; } = 1;
        public int Health { get; private set; } = 100;

        public bool IsAlive => Health > 0;
        public bool IsDead => Health <= 0;

        public virtual void ShowInfo()
        {
            Console.WriteLine($"Имя: {Name}");
            Console.WriteLine($"Урон: {Damage}");
        }

        public virtual object Clone()
        {
            return new Fighter(Name, Damage);
        }

        public virtual void TakeDamage(int damage)
        {
            Health -= damage;
            Console.WriteLine($"{Name} остаток жизни: {Health}");
        }

        public virtual void Attack(IDamageable damageable)
        {
            Attack(damageable, Damage);
        }

        public void Attack(IDamageable damageable, int damage)
        {
            damageable.TakeDamage(damage);
            Console.WriteLine($"{Name} бьет! Урон: {damage}");
        }
    }

    public interface IDamageable
    {
        void TakeDamage(int damage);
    }

    public class Arena
    {
        public void Fight(Fighter firstFighter, Fighter secondFighter)
        {
            // SetFirstMove(ref firstFighter, ref secondFighter);
            var round = 0;

            while (firstFighter.IsAlive && secondFighter.IsAlive)
            {
                Console.WriteLine($"Раунд {++round}!");
                firstFighter.Attack(secondFighter);
                secondFighter.Attack(firstFighter);
                Console.WriteLine();
            }

            if (firstFighter.IsDead && secondFighter.IsDead)
            {
                Console.WriteLine($"Ничья!");
                return;
            }

            var winner = firstFighter.IsAlive ? firstFighter : secondFighter;

            Console.WriteLine($"Победил {winner}");
        }

        private static void SetFirstMove(ref Fighter firstFighter, ref Fighter secondFighter)
        {
            var changeFirstMove = UserUtils.GenerateRandomBool();

            if (changeFirstMove)
            {
                var temp = firstFighter;
                firstFighter = secondFighter;
                secondFighter = temp;
            }
        }
    }

    public class UserUtils
    {
        private static Random s_random = new Random();

        public static bool GenerateRandomBool()
        {
            return s_random.Next(2) == 0;
        }

        public static int GenerateRandomNumber(int max)
        {
            return s_random.Next(max + 1);
        }
    }

    public class ArenaDungeons
    {
        public static List<Fighter> GetAllFighters()
        {
            var list = new List<Fighter>
            {
                new FighterDualAttack("Спартак", 10),
                new Fighter("Коммод", 12),
                new Fighter("Кавендиш", 12),
                new Fighter("Максимус", 20),
                new Fighter("Фламма ", 30),
            };

            return list;
        }
    }
}