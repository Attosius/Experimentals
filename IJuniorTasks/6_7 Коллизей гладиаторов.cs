using System;
using System.Collections.Generic;
using System.Threading;

namespace IJuniorTasks67
{
    public interface IDamageable
    {
        void TakeDamage(int damage);
    }

    internal class Program67
    {
        static void Main67(string[] args)
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

            if (TryGetFighter(out Fighter firstFighter) == false)
            {
                Console.ReadLine();
                return;
            }

            UserUtils.WriteLine($"Выберите второго бойца:", ConsoleColor.Cyan);

            if (TryGetFighter(out Fighter secondFighter) == false)
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
                Console.Write($"{++index}. {fighter.Name,-12} ");
                fighter.ShowAbility();
            }
        }

        private bool TryGetFighter(out Fighter fighter)
        {
            fighter = default;

            while (fighter == default)
            {
                ShowFighters();
                var fighterIndexStr = Console.ReadLine();

                if (int.TryParse(fighterIndexStr, out var fighterIndex) == false || fighterIndex < 0 || fighterIndex > _fighters.Count)
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
                UserUtils.WriteLine($"\nРаунд {++round}!", ConsoleColor.DarkGray);

                firstFighter.Attack(secondFighter);
                Console.WriteLine();
                secondFighter.Attack(firstFighter);

                Console.WriteLine("\n=====");
                firstFighter.PaintStats();
                secondFighter.PaintStats();
                Console.WriteLine("=====");

                Thread.Sleep(300);
            }

            ShowFightResult(firstFighter, secondFighter);
        }

        private static void ShowFightResult(Fighter firstFighter, Fighter secondFighter)
        {
            if (firstFighter.IsDead && secondFighter.IsDead)
            {
                Console.WriteLine($"Ничья!");
                return;
            }

            var winner = firstFighter.IsAlive ? firstFighter : secondFighter;
            UserUtils.WriteLine($"\nПобедил {winner.Name}!", ConsoleColor.Cyan);
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
        public int Health { get; protected set; }
        public int MinDamage { get; }
        public int MaxDamage { get; }
        public int Armor { get; }

        public bool IsAlive => Health > 0;
        public bool IsDead => Health <= 0;

        public virtual void ShowInfo()
        {
            Console.WriteLine($"Имя: {Name}");
            Console.WriteLine($"Жизни: {Health}");
            Console.WriteLine($"Минимальный урон: {MinDamage}");
            Console.WriteLine($"Максимальный урон: {MaxDamage}");
            Console.WriteLine($"Броня: {Armor}");
            ShowAbility();
        }

        public virtual void ShowAbility()
        {
            Console.WriteLine($"Способность: отсуствует");
        }

        public virtual void PaintStats()
        {
            var width = Health / (double)MaxHealth * BarWidth;
            var color = ConsoleColor.Green;
            PaintStat("Жизнь", color, width);
        }

        public void PaintStat(string barName, ConsoleColor color, double width)
        {
            const string FullBlockSymbol = "\u2588";
            const string VerticalSymbol = "\u2502";

            Console.Write($"{Name,-10} {barName,-12}: ");
            UserUtils.Write(VerticalSymbol, ConsoleColor.White);

            for (int i = 0; i < BarWidth; i++)
            {
                if (i < width)
                {
                    UserUtils.Write(FullBlockSymbol, color);
                }
                else
                {
                    UserUtils.Write(" ", color);
                }
            }

            UserUtils.WriteLine(VerticalSymbol, ConsoleColor.White);
        }

        public virtual object Clone()
        {
            var obj = this.MemberwiseClone();
            return (Fighter)obj;
        }

        public virtual void TakeDamage(int damage)
        {
            var finishDamage = Math.Max(damage - Armor, 0);
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

        protected void Attack(IDamageable damageable, int damage, bool incrementAttackCount = true)
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

        protected void IncrementAttackCount()
        {
            AttackCount++;
        }
    }

    public class FighterDoubleDamage : Fighter
    {
        public FighterDoubleDamage(string name, int minDamage, int maxDamage, int armor, int health, int doubleDamageAttackPercent)
            : base(name, minDamage, maxDamage, armor, health)
        {
            DoubleDamageAttackPercent = doubleDamageAttackPercent;
        }

        public int DoubleDamageAttackPercent { get; }

        public override void ShowAbility()
        {
            Console.WriteLine($"Способность: двойной урон с вероятностью {DoubleDamageAttackPercent}%");
        }

        public override void Attack(IDamageable damageable)
        {
            const int DamageMultiplier = 2;

            var damage = UserUtils.GenerateRandomNumber(MinDamage, MaxDamage);

            if (DoubleDamageAttackPercent > UserUtils.GenerateRandomNumber(MaxPercent))
            {
                damage *= DamageMultiplier;
                UserUtils.WriteLine($"\n{Name}: Двойной урон! ", ConsoleColor.Red);
            }

            base.Attack(damageable, damage);
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

        public override void ShowAbility()
        {
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

        public override void ShowAbility()
        {
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
            var widthPart = AttackCount % DualAttackPeriod;
            widthPart = widthPart == 0 ? DualAttackPeriod : widthPart;
            var width = widthPart / (double)DualAttackPeriod * BarWidth;
            var color = ConsoleColor.DarkYellow;
            PaintStat("Вторая атака", color, width);
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

        public override void ShowAbility()
        {
            Console.WriteLine($"Способность: получая урон накапливает {RageByTakeDamage} ярости. Накопив {RageLimit}, лечится на {HealthByRage} здоровья");
        }

        public override void PaintStats()
        {
            base.PaintStats();
            var width = _rage / (double)RageLimit * BarWidth;
            var color = ConsoleColor.DarkYellow;
            PaintStat("Ярость", color, width);
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
            MaxMana = mana;
            Mana = MaxMana;
            DamageByFireball = damageByFireball;
            ManaCostFireball = manaCostFireball;
        }

        public int Mana { get; private set; }
        public int MaxMana { get; }
        public int ManaCostFireball { get; }
        public int DamageByFireball { get; }

        public override void ShowAbility()
        {
            Console.WriteLine($"Способность: огненный шар с уроном {DamageByFireball} требует {ManaCostFireball} маны. Всего маны: {Mana}");
        }

        public override void Attack(IDamageable damageable)
        {
            int damage;

            if (Mana >= ManaCostFireball)
            {
                Mana -= ManaCostFireball;
                UserUtils.WriteLine($"\n{Name}: Способность Огненный шар! ", ConsoleColor.Red);
                damage = DamageByFireball;
            }
            else
            {
                damage = UserUtils.GenerateRandomNumber(MinDamage, MaxDamage);
            }

            base.Attack(damageable, damage);
        }

        public override void PaintStats()
        {
            base.PaintStats();
            var width = Mana / (double)MaxMana * BarWidth;
            var color = ConsoleColor.Blue;
            PaintStat("Мана", color, width);
        }

        public override object Clone()
        {
            return (FighterMagician)base.Clone();
        }
    }

    public class FighterDodge : Fighter
    {
        public FighterDodge(string name, int minDamage, int maxDamage, int armor, int health, int dodgePercent)
            : base(name, minDamage, maxDamage, armor, health)
        {
            DodgePercent = dodgePercent;
        }

        public int DodgePercent { get; }

        public override void ShowAbility()
        {
            Console.WriteLine($"Способность: возможность уклонения с вероятностью {DodgePercent}%");
        }

        public override void TakeDamage(int damage)
        {
            if (DodgePercent > UserUtils.GenerateRandomNumber(MaxPercent))
            {
                UserUtils.Write($"{Name}: Уклоняется от удара! ", ConsoleColor.Red);
                Console.Write($"Остаток жизни: ");
                UserUtils.WriteLine($"{Health}", ConsoleColor.Green);
                return;
            }

            base.TakeDamage(damage);
        }

        public override object Clone()
        {
            return (FighterDodge)base.Clone();
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
                new FighterMagician("Фламма ", 15, 20, 10, 100, mana: 60, damageByFireball: 28, manaCostFireball : 15),
                new FighterDodge("Уклонимус", 15, 20, 10, 100, dodgePercent: 30),
            };

            return list;
        }
    }
}