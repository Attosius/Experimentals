using System;
using System.Collections.Generic;

namespace IJuniorTasks
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var administrator = new Administrator();
            administrator.Run();
        }
    }

    public enum Commands
    {
        AcceptCar = 1,
        ShowQueue,
        ShowWarehouse,
        ShowBalance,
        RepairCar,
        RefuseRepair,
        Exit
    }

    public class Administrator
    {
        public void Run()
        {
            bool isWork = true;
            var autoService = new AutoService();

            while (isWork)
            {
                Console.Clear();
                autoService.ShowStats();

                Console.WriteLine($"\n\nВведите команду:");
                Console.WriteLine($"{(int)Commands.AcceptCar}. Принять машину");
                Console.WriteLine($"{(int)Commands.ShowQueue}. Показать очередь");
                Console.WriteLine($"{(int)Commands.ShowWarehouse}. Показать склад");
                Console.WriteLine($"{(int)Commands.ShowBalance}. Показать баланс");
                Console.WriteLine($"{(int)Commands.RepairCar}. Починить машину");
                Console.WriteLine($"{(int)Commands.RefuseRepair}. Отказаться от ремонта");
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
                    case Commands.AcceptCar:
                        autoService.AcceptCar();
                        break;

                    case Commands.ShowQueue:
                        autoService.ShowQueue();
                        break;

                    case Commands.ShowWarehouse:
                        autoService.ShowWarehouse();
                        break;

                    case Commands.ShowBalance:
                        autoService.ShowBalance();
                        break;

                    case Commands.RepairCar:
                        autoService.RepairCar();
                        break;

                    case Commands.RefuseRepair:
                        autoService.RefuseRepair();
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

    public class AutoService
    {
        private const int RefuseFineBeforeRepair = 500;
        private const int RefuseFinePerBrokenDetail = 300;
        private const int MaxQueueSize = 5;

        private readonly Queue<Car> _carQueue;
        private readonly Warehouse _warehouse;
        private readonly CarFactory _carFactory;
        private int _balance;

        public AutoService()
        {
            _balance = 10000;
            _warehouse = new Warehouse();
            _carFactory = new CarFactory();
            _carQueue = new Queue<Car>();
            _warehouse.Fill();
        }

        public void ShowStats()
        {
            Console.WriteLine("=== АВТОСЕРВИС ===");
            Console.Write($"Баланс: ");
            UserUtils.Write($"{_balance} руб.", ConsoleColor.Green);
            Console.WriteLine();
            Console.Write($"Машин в очереди: ");
            UserUtils.Write($"{_carQueue.Count}", ConsoleColor.Cyan);
            Console.WriteLine($" / {MaxQueueSize}");
            Console.Write($"Деталей на складе: ");
            UserUtils.Write($"{_warehouse.GetTotalCount()}", ConsoleColor.Yellow);
            Console.WriteLine();
        }

        public void AcceptCar()
        {
            if (_carQueue.Count >= MaxQueueSize)
            {
                Console.WriteLine($"Очередь заполнена! Максимум {MaxQueueSize} машин.");
                Console.ReadKey();
                return;
            }

            var car = _carFactory.CreateRandomCar();
            _carQueue.Enqueue(car);
            Console.WriteLine($"Машина принята в очередь на ремонт.");
            UserUtils.Write(car.Name, ConsoleColor.Cyan);
            Console.WriteLine();
            car.ShowBrokenDetails();

            var breakCount = car.GetBrokenDetailsCount();
            var potentialRevenue = breakCount * 1000;
            Console.Write($"Потенциальная выручка за полный ремонт: ");
            UserUtils.Write($"{potentialRevenue} руб.", ConsoleColor.Green);
            Console.WriteLine();

            Console.ReadKey();
        }

        public void ShowQueue()
        {
            Console.Clear();
            Console.WriteLine("=== ОЧЕРЕДЬ МАШИН ===");

            if (_carQueue.Count == 0)
            {
                Console.WriteLine("Очередь пуста.");
                Console.ReadKey();
                return;
            }

            var index = 1;
            foreach (var car in _carQueue)
            {
                Console.Write($"{index}. ");
                UserUtils.Write($"{car.Name}", ConsoleColor.Cyan);
                Console.Write($" - ");
                UserUtils.Write($"{car.GetBrokenDetailsCount()}", ConsoleColor.Red);
                Console.WriteLine($" поломанных деталей");

                car.ShowBrokenDetails();
                Console.WriteLine();
                index++;
            }

            Console.ReadKey();
        }

        public void ShowWarehouse()
        {
            Console.Clear();
            Console.WriteLine("=== СКЛАД ДЕТАЛЕЙ ===");
            _warehouse.ShowDetails();
            Console.ReadKey();
        }

        public void ShowBalance()
        {
            Console.Clear();
            Console.WriteLine("=== БАЛАНС ===");
            Console.Write($"Текущий баланс: ");
            UserUtils.Write($"{_balance} руб.", ConsoleColor.Green);
            Console.WriteLine();
            Console.ReadKey();
        }

        public void RepairCar()
        {
            if (_carQueue.Count == 0)
            {
                Console.WriteLine("Очередь пуста. Нет машин для ремонта.");
                Console.ReadKey();
                return;
            }

            var car = _carQueue.Peek();

            if (car.GetBrokenDetailsCount() == 0)
            {
                Console.WriteLine("Машина полностью исправна!");
                _carQueue.Dequeue();
                Console.ReadKey();
                return;
            }

            Console.Clear();
            Console.WriteLine("=== РЕМОНТ МАШИНЫ ===");
            UserUtils.Write(car.Name, ConsoleColor.Cyan);
            Console.WriteLine();
            car.ShowBrokenDetails();

            Console.WriteLine("\nВыберите деталь для замены:");
            var brokenDetails = car.GetBrokenDetails();

            for (int i = 0; i < brokenDetails.Count; i++)
            {
                var detail = brokenDetails[i];
                Console.Write($"{i + 1}. ");
                UserUtils.Write($"{detail.DetailType}", ConsoleColor.Yellow);
                Console.Write($" - выплата: ");
                UserUtils.Write($"{detail.RepairPrice}", ConsoleColor.Green);
                Console.WriteLine($" руб. (на складе: {_warehouse.GetDetailCount(detail.DetailType)})");
            }

            var choiceText = Console.ReadLine();

            if (int.TryParse(choiceText, out var choice) == false || choice > brokenDetails.Count || choice < 1)
            {
                Console.WriteLine("Некорректный выбор!");
                Console.ReadKey();
                return;
            }

            var selectedDetail = brokenDetails[choice - 1];

            if (_warehouse.GetDetailCount(selectedDetail.DetailType) == 0)
            {
                Console.WriteLine("Этой детали нет на складе!");
                Console.ReadKey();
                return;
            }

            car.ReplaceDetail(selectedDetail);
            _warehouse.RemoveDetail(selectedDetail.DetailType);
            _balance += selectedDetail.RepairPrice;

            Console.Write("\nДеталь ");
            UserUtils.Write($"{selectedDetail.DetailType}", ConsoleColor.Yellow);
            Console.WriteLine(" заменена.");
            Console.Write("Выплата за ремонт: ");
            UserUtils.Write($"+{selectedDetail.RepairPrice}", ConsoleColor.Green);
            Console.WriteLine($" руб.");

            if (car.GetBrokenDetailsCount() == 0)
            {
                _carQueue.Dequeue();
                UserUtils.WriteLine("\nРЕМОНТ ЗАВЕРШЕН! Машина взята из очереди.", ConsoleColor.Green);
            }

            Console.ReadKey();
        }

        public void RefuseRepair()
        {
            if (_carQueue.Count == 0)
            {
                Console.WriteLine("Очередь пуста.");
                Console.ReadKey();
                return;
            }

            Console.Clear();
            Console.WriteLine("=== ОТКАЗ ОТ РЕМОНТА ===");
            Console.WriteLine("1. Отказаться от ремонта первой машины (перед починкой)");
            Console.WriteLine("2. Отказаться от ремонта первой машины (во время починки)");
            Console.WriteLine();

            var choiceText = Console.ReadLine();

            if (int.TryParse(choiceText, out var choice) == false || choice < 1 || choice > 2)
            {
                Console.WriteLine("Некорректный выбор!");
                Console.ReadKey();
                return;
            }

            var car = _carQueue.Peek();

            if (choice == 1)
            {
                _carQueue.Dequeue();
                _balance -= RefuseFineBeforeRepair;

                Console.Write("Штраф за отказ от ремонта: ");
                UserUtils.Write($"-{RefuseFineBeforeRepair}", ConsoleColor.Red);
                Console.WriteLine($" руб.");
            }
            else
            {
                var brokenCount = car.GetBrokenDetailsCount();
                var fine = brokenCount * RefuseFinePerBrokenDetail;
                _carQueue.Dequeue();
                _balance -= fine;

                Console.Write($"Штраф за непочиненные детали ({brokenCount}): ");
                UserUtils.Write($"-{fine}", ConsoleColor.Red);
                Console.WriteLine($" руб.");
            }

            Console.ReadKey();
        }
    }

    public class Car
    {
        private readonly List<Detail> _details;

        public Car(string name)
        {
            Name = name;
            _details = new List<Detail>();
        }

        public string Name { get; }

        public void AddDetail(Detail detail)
        {
            _details.Add(detail);
        }

        public List<Detail> GetBrokenDetails()
        {
            var broken = new List<Detail>();
            foreach (var detail in _details)
            {
                if (detail.IsBroken)
                {
                    broken.Add(detail);
                }
            }
            return broken;
        }

        public int GetBrokenDetailsCount()
        {
            return GetBrokenDetails().Count;
        }

        public void ShowBrokenDetails()
        {
            var brokenDetails = GetBrokenDetails();

            if (brokenDetails.Count == 0)
            {
                Console.WriteLine("Поломок нет.");
                return;
            }

            Console.WriteLine("Поломанные детали:");
            foreach (var detail in brokenDetails)
            {
                UserUtils.Write($"  • {detail.DetailType}", ConsoleColor.Red);
                Console.WriteLine($" - {detail.RepairPrice} руб.");
            }
        }

        public void ReplaceDetail(Detail brokenDetail)
        {
            for (int i = 0; i < _details.Count; i++)
            {
                if (_details[i] == brokenDetail)
                {
                    _details[i] = new Detail(brokenDetail.DetailType, brokenDetail.RepairPrice, false);
                    break;
                }
            }
        }
    }

    public class Detail
    {
        public Detail(DetailType detailType, int repairPrice, bool isBroken)
        {
            DetailType = detailType;
            RepairPrice = repairPrice;
            IsBroken = isBroken;
        }

        public DetailType DetailType { get; }
        public int RepairPrice { get; }
        public bool IsBroken { get; }
    }

    public enum DetailType
    {
        Двигатель,
        КоробкаПередач,
        МасляныйФильтр,
        ВоздушныйФильтр,
        СвечиЗажигания,
        ТормозныеКолодки,
        РулевойРейк,
        Радиатор,
        Аккумулятор,
        Генератор
    }

    public class Warehouse
    {
        private readonly Dictionary<DetailType, int> _details;
        private readonly Dictionary<DetailType, int> _repairPrices;

        public Warehouse()
        {
            _details = new Dictionary<DetailType, int>();
            _repairPrices = new Dictionary<DetailType, int>();

            InitializeRepairPrices();
        }

        private void InitializeRepairPrices()
        {
            _repairPrices.Add(DetailType.Двигатель, 5000);
            _repairPrices.Add(DetailType.КоробкаПередач, 4000);
            _repairPrices.Add(DetailType.МасляныйФильтр, 500);
            _repairPrices.Add(DetailType.ВоздушныйФильтр, 300);
            _repairPrices.Add(DetailType.СвечиЗажигания, 800);
            _repairPrices.Add(DetailType.ТормозныеКолодки, 1500);
            _repairPrices.Add(DetailType.РулевойРейк, 2500);
            _repairPrices.Add(DetailType.Радиатор, 2000);
            _repairPrices.Add(DetailType.Аккумулятор, 1000);
            _repairPrices.Add(DetailType.Генератор, 1500);
        }

        public void Fill()
        {
            _details.Clear();

            _details.Add(DetailType.Двигатель, 2);
            _details.Add(DetailType.КоробкаПередач, 2);
            _details.Add(DetailType.МасляныйФильтр, 5);
            _details.Add(DetailType.ВоздушныйФильтр, 5);
            _details.Add(DetailType.СвечиЗажигания, 8);
            _details.Add(DetailType.ТормозныеКолодки, 6);
            _details.Add(DetailType.РулевойРейк, 3);
            _details.Add(DetailType.Радиатор, 3);
            _details.Add(DetailType.Аккумулятор, 4);
            _details.Add(DetailType.Генератор, 4);
        }

        public int GetDetailCount(DetailType detailType)
        {
            return _details.ContainsKey(detailType) ? _details[detailType] : 0;
        }

        public int GetTotalCount()
        {
            int total = 0;
            foreach (var count in _details.Values)
            {
                total += count;
            }
            return total;
        }

        public void RemoveDetail(DetailType detailType)
        {
            if (_details.ContainsKey(detailType) && _details[detailType] > 0)
            {
                _details[detailType]--;
            }
        }

        public void ShowDetails()
        {
            Console.WriteLine();

            foreach (var detail in _details)
            {
                Console.Write($"  • {detail.Key}: ");
                UserUtils.Write($"{detail.Value}", ConsoleColor.Yellow);
                Console.WriteLine($" шт. (выплата: {_repairPrices[detail.Key]} руб./шт.)");
            }
        }

        public int GetRepairPrice(DetailType detailType)
        {
            return _repairPrices.ContainsKey(detailType) ? _repairPrices[detailType] : 0;
        }
    }

    public class CarFactory
    {
        private static readonly string[] s_carNames =
        {
            "Toyota Camry",
            "BMW X5",
            "Mercedes E-Class",
            "Audi A4",
            "Ford Mustang",
            "Honda Accord"
        };

        private static readonly Random s_random = new();

        public Car CreateRandomCar()
        {
            var name = s_carNames[s_random.Next(s_carNames.Length)];
            var car = new Car(name);

            var allDetailTypes = (DetailType[])Enum.GetValues(typeof(DetailType));
            var detailCount = s_random.Next(3, 6);
            var brokenCount = s_random.Next(1, detailCount + 1);

            var selectedDetails = new List<DetailType>();

            while (selectedDetails.Count < detailCount)
            {
                var randomDetail = allDetailTypes[s_random.Next(allDetailTypes.Length)];
                if (!selectedDetails.Contains(randomDetail))
                {
                    selectedDetails.Add(randomDetail);
                }
            }

            var warehouse = new Warehouse();

            for (int i = 0; i < selectedDetails.Count; i++)
            {
                bool isBroken = i < brokenCount;
                var price = warehouse.GetRepairPrice(selectedDetails[i]);
                car.AddDetail(new Detail(selectedDetails[i], price, isBroken));
            }

            return car;
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
