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

        private readonly List<Fighter> _fighters;

        public Coliseum()
        {
            _fighters = ArenaDungeons.GetAllFighters();
        }

        public void ShowWelcomeMessage()
        {
            Console.WriteLine("Ave, Caesar, morituri te salutant!");
            Console.WriteLine("Нажмите Enter для продолжения...");
            Console.ReadKey();
        }

        public void CreateFight()
        {
            UserUtils.WriteLine("Выберите первого бойца:", ConsoleColor.Cyan);

            if (!TryGetFighter(out Fighter firstFighter))
            {
                Console.ReadLine();
                return;
            }

            UserUtils.WriteLine($"Выберите второго бойца:", ConsoleColor.Cyan);

            if (!TryGetFighter(out Fighter secondFighter))
            {
                Console.ReadKey();
                return;
            }

            var arena = new Arena();
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

                UserUtils.WriteLine($"Вы выбрали {currentFighter.Name}! Его характеристики:", ConsoleColor.Cyan);
                currentFighter.ShowInfo();
                Console.WriteLine($"\nПродолжить с этим бойцом? (y/n)");
                var answer = Console.ReadLine();

                if (string.IsNullOrEmpty(answer) || answer.Equals("y", StringComparison.OrdinalIgnoreCase))
                {
                    fighter = currentFighter;
                    return true;
                }

                Console.WriteLine($"Выберите бойца:");
            }

            return true;
        }
    }

    public class Arena
    {
        public void Fight(Fighter firstFighter, Fighter secondFighter)
        {
            UserUtils.WriteLine($"Бой между {firstFighter.Name} и {secondFighter.Name}!", ConsoleColor.Cyan);
            Thread.Sleep(500);

            var round = 0;

            while (firstFighter.IsAlive && secondFighter.IsAlive)
            {
                UserUtils.WriteLine($"Раунд {++round}!", ConsoleColor.DarkGray);

                firstFighter.Attack(secondFighter);
                Console.WriteLine();

                secondFighter.Attack(firstFighter);
                Console.WriteLine();
                firstFighter.PaintStats();
                secondFighter.PaintStats();
                Console.WriteLine();
                Thread.Sleep(300);
            }

            if (firstFighter.IsDead && secondFighter.IsDead)
            {
                Console.WriteLine($"Ничья!");
                return;
            }

            var winner = firstFighter.IsAlive ? firstFighter : secondFighter;
            Console.WriteLine($"Победил {winner.Name}!");
        }
    }
   
    
    public class Fighter : ICloneable, IDamageable
    {
        public const int MaxPercent = 100;

        protected int AttackCount;
        protected int MaxHealth;
        protected int BarWidth = 20;

        public Fighter(string name, int minDamage, int maxDamage, int armor, int health)
        {
            Name = name;
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            Armor = armor;
            Health = health;
            MaxHealth = health;
        }

        public string Name { get; }
        public int MinDamage { get; }
        public int MaxDamage { get; }
        public int Armor { get;  }
        public int Health { get; protected set; }

        public bool IsAlive => Health > 0;
        public bool IsDead => Health <= 0;

        public virtual void ShowInfo()
        {
            Console.WriteLine($"Имя: {Name}");
            Console.WriteLine($"Жизни: {Health}");
            Console.WriteLine($"Минимальный урон: {MinDamage}");
            Console.WriteLine($"Максимальный урон: {MaxDamage}");
            Console.WriteLine($"Броня: {Armor}");
        }

        public virtual void PaintStats()
        {
            var width = Health / (double)MaxHealth * BarWidth;
            var color = ConsoleColor.Green;
            PaintStat(color, width);
        }

        protected void PaintStat(ConsoleColor color, double width)
        {
            const string fullBlockSymbol = "\u2588";
            const string verticalSymbol = "\u2502";

            Console.Write($"{Name,-10}: ");
            UserUtils.Write(verticalSymbol, ConsoleColor.White);

            for (int i = 0; i < BarWidth; i++)
            {
                if (i < width)
                {
                    UserUtils.Write(fullBlockSymbol, color);
                }
                else
                {
                    UserUtils.Write(" ", color);
                }
            }
            UserUtils.WriteLine(verticalSymbol, ConsoleColor.White);
        }

        public virtual object Clone()
        {
            var obj = this.MemberwiseClone();
            return (Fighter)obj;
        }

        public virtual void TakeDamage(int damage)
        {
            var finishDamage = damage - Armor;
            Console.Write($"{Name}: Броня: {Armor}. Итоговый урон: {finishDamage}. ");
            Health -= finishDamage;
            Console.Write($"Остаток жизни: ");
            UserUtils.WriteLine($"{Health}", ConsoleColor.Green);
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

        protected void IncrementAttackCount()
        {
            AttackCount++;
        }

        private void Attack(IDamageable damageable, int damage, bool incrementAttackCount)
        {
            Console.Write($"{Name}: Бьет! Урон: ");
            UserUtils.Write(damage.ToString(), ConsoleColor.Red);
            Console.WriteLine();
            damageable.TakeDamage(damage);

            if (incrementAttackCount)
            {
                IncrementAttackCount();
            }
        }
    }

    public class FighterDoubleDamage : Fighter
    {
        public FighterDoubleDamage(string name, int minDamage, int maxDamage, int armor, int health, int doubleDamageAttackPercent)
            : base(name, minDamage, maxDamage, armor, health)
        {
            DoubleDamageAttackPercent = doubleDamageAttackPercent;
        }

        public override void ShowInfo()
        {
            base.ShowInfo();
            Console.WriteLine($"Способность: двойной урон с вероятностью {DoubleDamageAttackPercent}%");
        }

        public int DoubleDamageAttackPercent { get; }

        public override void Attack(IDamageable damageable)
        {
            var damage = UserUtils.GenerateRandomNumber(MinDamage, MaxDamage);

            Console.Write($"{Name}: Бьет! ");
            if (DoubleDamageAttackPercent > UserUtils.GenerateRandomNumber(MaxPercent))
            {
                damage *= 2;
                UserUtils.WriteLine($"\n{Name}: Двойной удар! ", ConsoleColor.Red);
            }

            Console.Write("Урон: ");
            UserUtils.WriteLine(damage.ToString(), ConsoleColor.Red);
            damageable.TakeDamage(damage);
            IncrementAttackCount();
        }

        public override object Clone()
        {
            return (FighterDoubleDamage)base.Clone();
        }
    }

    public class FighterDualAttackPercently : Fighter
    {
        public FighterDualAttackPercently(string name, int minDamage, int maxDamage, int armor, int health, int dualAttackPercent)
            : base(name, minDamage, maxDamage, armor, health)
        {
            DualAttackPercent = dualAttackPercent;
        }

        public int DualAttackPercent { get; }

        public override void ShowInfo()
        {
            base.ShowInfo();
            Console.WriteLine($"Способность: вторая атака с вероятностью {DualAttackPercent}%");
        }

        public override void Attack(IDamageable damageable)
        {
            base.Attack(damageable);

            if (DualAttackPercent > UserUtils.GenerateRandomNumber(MaxPercent))
            {
                UserUtils.WriteLine($"\n{Name}: Вторая атака! ", ConsoleColor.Red);
                base.Attack(damageable);
            }
        }

        public override object Clone()
        {
            return (FighterDualAttackPercently)base.Clone();
        }
    }

    public class FighterDualAttackPeriodically : Fighter
    {
        public FighterDualAttackPeriodically(string name, int minDamage, int maxDamage, int armor, int health, int dualAttackPeriod)
            : base(name, minDamage, maxDamage, armor, health)
        {
            DualAttackPeriod = dualAttackPeriod;
        }
        public int DualAttackPeriod { get; }

        public override void ShowInfo()
        {
            base.ShowInfo();
            Console.WriteLine($"Способность: вторая атака каждую {DualAttackPeriod} атаку");
        }


        public override void Attack(IDamageable damageable)
        {
            base.Attack(damageable);

            if (AttackCount % DualAttackPeriod == 0)
            {
                UserUtils.WriteLine($"\n{Name}: Дополнительная атака! ", ConsoleColor.Red);
                base.Attack(damageable, false);
            }
        }

        public override void PaintStats()
        {
            base.PaintStats();
            var maxWidth = 20;
            var widthPart = AttackCount % DualAttackPeriod;
            widthPart = widthPart == 0 ? DualAttackPeriod : widthPart;
            var width = widthPart / (double)DualAttackPeriod * BarWidth;
            var color = ConsoleColor.DarkYellow;
            PaintStat(color, width);
        }

        public override object Clone()
        {
            return (FighterDualAttackPeriodically)base.Clone();
        }
    }

    public class FighterRage : Fighter
    {
        private int _rage;

        public FighterRage(string name, int minDamage, int maxDamage, int armor, int health, int rageByTakeDamage, int rageLimit, int healthByRage)
            : base(name, minDamage, maxDamage, armor, health)
        {
            RageByTakeDamage = rageByTakeDamage;
            RageLimit = rageLimit;
            HealthByRage = healthByRage;
        }

        public int RageByTakeDamage { get; }
        public int RageLimit { get; }
        public int HealthByRage { get; }

        public override void ShowInfo()
        {
            base.ShowInfo();
            Console.WriteLine($"Способность: получая урон накапливает {RageByTakeDamage} ярости. Накопив {RageLimit}, лечится на {HealthByRage} здоровья");
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            _rage += RageByTakeDamage;
            Console.WriteLine($"{Name}: Накоплено {_rage} ярости! ");

            if (_rage >= RageLimit)
            {
                _rage = 0;
                UserUtils.Write($"{Name}: Лимит ярости достигнут! ", ConsoleColor.Red);
                Health += HealthByRage;

                if (Health > MaxHealth)
                {
                    Health = MaxHealth;
                }

                Console.Write($"Лечение на {HealthByRage} ");
                Console.Write($"Остаток жизни: ");
                UserUtils.WriteLine($"{Health}", ConsoleColor.Green);
            }

        }

        public override object Clone()
        {
            return (FighterRage)base.Clone();
        }
    }


    public class FighterMagician : Fighter
    {
        public FighterMagician(string name, int minDamage, int maxDamage, int armor, int health, int mana, int damageByFireball, int manaCostFireball)
            : base(name, minDamage, maxDamage, armor, health)
        {
            Mana = mana;
            DamageByFireball = damageByFireball;
            ManaCostFireball = manaCostFireball;
        }

        public int Mana { get; private set; }
        public int DamageByFireball { get; }
        public int ManaCostFireball { get; }

        public override void ShowInfo()
        {
            base.ShowInfo();
            Console.WriteLine($"Способность: Огненный шар с уроном {DamageByFireball} требует {ManaCostFireball} маны. Всего маны {Mana}");
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);


            Mana -= ManaCostFireball;
            Console.WriteLine($"{Name}: Накоплено {_mana} ярости! ");

            if (_mana >= DamageByFireball)
            {
                _mana = 0;
                UserUtils.Write($"{Name}: Лимит ярости достигнут! ", ConsoleColor.Red);
                Health += ManaCostFireball;

                if (Health > MaxHealth)
                {
                    Health = MaxHealth;
                }

                Console.Write($"Лечение на {ManaCostFireball} ");
                Console.Write($"Остаток жизни: ");
                UserUtils.WriteLine($"{Health}", ConsoleColor.Green);
            }

        }

        public override object Clone()
        {
            return (FighterMagician)base.Clone();
        }
    }

    public interface IDamageable
    {
        void TakeDamage(int damage);
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

    public class ArenaDungeons
    {
        public static List<Fighter> GetAllFighters()
        {
            var list = new List<Fighter>
            {
                new FighterDoubleDamage("Спартак", minDamage: 15, maxDamage: 20, armor: 10, health: 100, doubleDamageAttackPercent: 30),
                new FighterDualAttackPercently("Коммод", 15, 20, 10, 100, dualAttackPercent: 30),
                new FighterDualAttackPeriodically("Aвендиш", 15, 20, 10, 100, dualAttackPeriod: 3),
                new FighterRage("Максимус", 15, 20, 10, 100, rageByTakeDamage: 10, rageLimit: 30, healthByRage: 15),
                new Fighter("Фламма ", 15, 20, 10, 100),
            };

            return list;
        }
    }
}