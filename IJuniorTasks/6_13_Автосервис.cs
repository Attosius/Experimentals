using System;
using System.Collections.Generic;

namespace IJuniorTasks613
{
    internal class Program
    {
        public static void Main613(string[] args)
        {
            var administrator = new Administrator();
            administrator.Run();
        }
    }

    public enum Commands
    {
        AcceptCar = 1,
        ShowCarInService,
        ShowStock,
        ShowBalance,
        Exit,
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
                autoService.ShowMenu();

                Console.WriteLine($"\nВведите команду:");
                Console.WriteLine($"{(int)Commands.AcceptCar}. Принять машину");
                Console.WriteLine($"{(int)Commands.ShowCarInService}. Показать машину в ремонте");
                Console.WriteLine($"{(int)Commands.ShowStock}. Показать склад");
                Console.WriteLine($"{(int)Commands.ShowBalance}. Показать баланс");
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

                    case Commands.ShowCarInService:
                        autoService.ShowCarInService();
                        break;

                    case Commands.ShowStock:
                        autoService.ShowStock();
                        break;

                    case Commands.ShowBalance:
                        autoService.ShowBalance();
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
        private readonly int _repairPrice = 500;
        private readonly int _refusalFineBeforeRepair = 100;
        private readonly int _refusalFinePerBrokenPart = 200;

        private readonly CarFactory _carFactory;
        private readonly Stock _stock;
        private readonly Queue<Car> _carsQueue;

        private Car _currentCar;

        public AutoService()
        {
            _carFactory = new CarFactory();
            _stock = new Stock();
            _carsQueue = new Queue<Car>();
        }

        public int Balance { get; private set; }

        public void ShowMenu()
        {
            Console.WriteLine("=== АВТОСЕРВИС ===");
            Console.WriteLine($"Баланс: {Balance}");
            Console.WriteLine($"Машин в очереди: {_carsQueue.Count}");

            if (_currentCar != null)
            {
                Console.Write($"В ремонте: машина с ");
                UserUtils.Write($"{_currentCar.GetBrokenPartsCount()}", ConsoleColor.Red);
                Console.WriteLine($" поломками");
            }
        }

        public void AcceptCar()
        {
            var car = _carFactory.CreateRandomCar();
            _carsQueue.Enqueue(car);

            Console.WriteLine($"Принята новая машина с {car.GetBrokenPartsCount()} поломками");
            Console.ReadKey();
        }

        public void ShowCarInService()
        {
            if (_currentCar == null)
            {
                if (_carsQueue.Count == 0)
                {
                    Console.WriteLine($"Нет машин в очереди и в ремонте");
                    Console.ReadKey();
                    return;
                }

                _currentCar = _carsQueue.Dequeue();
                Console.WriteLine($"Мachine поставлена на ремонт");
                ShowCarInfo();
            }
            else
            {
                ShowCarInfo();
                RepairMenu();
            }
        }

        public void ShowStock()
        {
            Console.WriteLine("=== СКЛАД ДЕТАЛЕЙ ===");
            _stock.ShowParts();
            Console.ReadKey();
        }

        public void ShowBalance()
        {
            Console.WriteLine($"=== БАЛАНС ===");
            Console.Write($"Баланс автосервиса: ");
            UserUtils.WriteLine($"{Balance}", ConsoleColor.Green);
            Console.ReadKey();
        }

        private void ShowCarInfo()
        {
            Console.Clear();
            Console.WriteLine("=== МАШИНА В РЕМОНТЕ ===");
            Console.WriteLine();

            var brokenParts = _currentCar.GetBrokenParts();

            if (brokenParts.Count == 0)
            {
                Console.WriteLine("Все детали исправны!");
                return;
            }

            Console.WriteLine("Поломанные детали:");

            for (int i = 0; i < brokenParts.Count; i++)
            {
                var part = brokenParts[i];
                Console.Write($"{i + 1}. ");
                UserUtils.Write($"{part.Name}", ConsoleColor.Red);
                Console.Write($" - цена детали: ");
                UserUtils.WriteLine($"{part.Price}", ConsoleColor.Yellow);
            }
        }

        private void RepairMenu()
        {
            Console.WriteLine();
            Console.WriteLine("Действия:");
            Console.WriteLine("1. Починить деталь");
            Console.WriteLine("2. Отказаться от ремонта");
            Console.WriteLine();
            Console.Write("Ваш выбор: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    RepairPart();
                    break;
                case "2":
                    RefuseRepair();
                    break;
                default:
                    Console.WriteLine("Некорректный выбор!");
                    Console.ReadKey();
                    break;
            }
        }

        private void RepairPart()
        {
            var brokenParts = _currentCar.GetBrokenParts();

            if (brokenParts.Count == 0)
            {
                Console.WriteLine("Все детали исправны!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine();
            Console.Write("Введите номер детали для замены: ");
            var indexText = Console.ReadLine();

            if (int.TryParse(indexText, out var index) == false || index > brokenParts.Count || index < 1)
            {
                Console.WriteLine("Некорректный номер детали!");
                Console.ReadKey();
                return;
            }

            var part = brokenParts[index - 1];

            if (_stock.HasPart(part))
            {
                _stock.RemovePart(part);
                _currentCar.ReplacePart(part);

                var repairCost = part.Price + _repairPrice;
                Balance += repairCost;

                Console.Write($"Деталь ");
                UserUtils.Write($"{part.Name}", ConsoleColor.Green);
                Console.WriteLine($" заменена");
                Console.Write($"Получено: ");
                UserUtils.WriteLine($"{repairCost}", ConsoleColor.Green);
                Console.Write($" (деталь: {part.Price} + ремонт: {_repairPrice})");
                Console.WriteLine();

                if (_currentCar.GetBrokenPartsCount() == 0)
                {
                    Console.WriteLine();
                    UserUtils.WriteLine("Ремонт завершен!", ConsoleColor.Green);
                    _currentCar = null;
                }
            }
            else
            {
                Console.Write($"Деталь ");
                UserUtils.Write($"{part.Name}", ConsoleColor.Red);
                Console.WriteLine($" отсутствует на складе!");
            }

            Console.ReadKey();
        }

        private void RefuseRepair()
        {
            var brokenPartsCount = _currentCar.GetBrokenPartsCount();

            if (brokenPartsCount == 0)
            {
                Console.WriteLine("Ремонт уже завершен!");
                Console.ReadKey();
                return;
            }

            int fine;
            var brokenParts = _currentCar.GetBrokenParts();
            bool wasRepaired = _currentCar.TotalParts > brokenParts.Count;

            Console.WriteLine();
            Console.WriteLine("Выберите тип отказа:");
            Console.WriteLine("1. Отказ перед ремонтом (штраф за все детали)");
            Console.WriteLine("2. Отказ во время ремонта (штраф за оставшиеся детали)");
            Console.WriteLine();
            Console.Write("Ваш выбор: ");

            var choice = Console.ReadLine();

            if (choice == "1")
            {
                fine = brokenPartsCount * _refusalFinePerBrokenPart;
                Console.Write($"Штраф за отказ перед ремонтом: ");
                UserUtils.WriteLine($"{fine}", ConsoleColor.Red);
            }
            else if (choice == "2")
            {
                if (wasRepaired == false)
                {
                    Console.WriteLine("Ремонт еще не начинался!");
                    Console.ReadKey();
                    return;
                }

                fine = brokenPartsCount * _refusalFinePerBrokenPart;
                Console.Write($"Штраф за отказ во время ремонта: ");
                UserUtils.WriteLine($"{fine}", ConsoleColor.Red);
            }
            else
            {
                Console.WriteLine("Некорректный выбор!");
                Console.ReadKey();
                return;
            }

            Balance -= fine;

            if (Balance < 0)
            {
                Balance = 0;
            }

            Console.Write($"Новый баланс: ");
            UserUtils.WriteLine($"{Balance}", ConsoleColor.Yellow);

            _currentCar = null;
            Console.ReadKey();
        }
    }

    public class Car
    {
        private readonly List<Part> _parts;

        public Car(List<Part> parts, List<bool> isDamagedList)
        {
            _parts = parts;

            for (int i = 0; i < _parts.Count; i++)
            {
                if (isDamagedList[i])
                {
                    _parts[i].Damage();
                }
            }
        }

        public int TotalParts => _parts.Count;

        public List<Part> GetBrokenParts()
        {
            var brokenParts = new List<Part>();

            foreach (var part in _parts)
            {
                if (part.IsDamaged)
                {
                    brokenParts.Add(part);
                }
            }

            return brokenParts;
        }

        public int GetBrokenPartsCount()
        {
            return GetBrokenParts().Count;
        }

        public void ReplacePart(Part part)
        {
            for (int i = 0; i < _parts.Count; i++)
            {
                if (_parts[i].Name == part.Name && _parts[i].IsDamaged)
                {
                    _parts[i].Repair();
                    return;
                }
            }
        }
    }

    public class Part
    {
        public Part(string name, int price)
        {
            Name = name;
            Price = price;
            IsDamaged = false;
        }

        public string Name { get; }
        public int Price { get; }
        public bool IsDamaged { get; private set; }

        public void Damage()
        {
            IsDamaged = true;
        }

        public void Repair()
        {
            IsDamaged = false;
        }
    }

    public class Stock
    {
        private readonly Dictionary<string, StockItem> _parts;

        public Stock()
        {
            _parts = new Dictionary<string, StockItem>();
            InitializeStock();
        }

        private void InitializeStock()
        {
            AddPart(new Part("Двигатель", 5000), 10);
            AddPart(new Part("Колесо", 500), 20);
            AddPart(new Part("Тормозная система", 1000), 15);
            AddPart(new Part("Аккумулятор", 300), 10);
            AddPart(new Part("Фара", 200), 8);
            AddPart(new Part("Рулевое управление", 1500), 12);
            AddPart(new Part("Коробка передач", 4000), 8);
            AddPart(new Part("Карбюратор", 800), 10);
        }

        private void AddPart(Part part, int quantity)
        {
            if (_parts.ContainsKey(part.Name))
            {
                _parts[part.Name].AddQuantity(quantity);
            }
            else
            {
                _parts[part.Name] = new StockItem(part, quantity);
            }
        }

        public bool HasPart(Part part)
        {
            return _parts.ContainsKey(part.Name) && _parts[part.Name].Quantity > 0;
        }

        public void RemovePart(Part part)
        {
            if (_parts.ContainsKey(part.Name))
            {
                _parts[part.Name].RemoveQuantity(1);
            }
        }

        public void ShowParts()
        {
            if (_parts.Count == 0)
            {
                Console.WriteLine("Склад пуст!");
                return;
            }

            foreach (var item in _parts.Values)
            {
                Console.Write($"- ");
                UserUtils.Write($"{item.Part.Name}", ConsoleColor.Cyan);
                Console.Write($" - цена: ");
                UserUtils.Write($"{item.Part.Price}", ConsoleColor.Yellow);
                Console.Write($" - количество: ");
                UserUtils.WriteLine($"{item.Quantity}", ConsoleColor.Green);
            }
        }
    }

    public class StockItem
    {
        public StockItem(Part part, int quantity)
        {
            Part = part;
            Quantity = quantity;
        }

        public Part Part { get; }
        public int Quantity { get; private set; }

        public void AddQuantity(int amount)
        {
            Quantity += amount;
        }

        public void RemoveQuantity(int amount)
        {
            Quantity -= amount;
            if (Quantity < 0)
            {
                Quantity = 0;
            }
        }
    }

    public class CarFactory
    {
        private readonly Random _random = new Random();
        private readonly List<Part> _availableParts = new List<Part>();

        public CarFactory()
        {
            InitializeParts();
        }

        private void InitializeParts()
        {
            _availableParts.Add(new Part("Двигатель", 5000));
            _availableParts.Add(new Part("Колесо", 500));
            _availableParts.Add(new Part("Тормозная система", 1000));
            _availableParts.Add(new Part("Аккумулятор", 300));
            _availableParts.Add(new Part("Фара", 200));
            _availableParts.Add(new Part("Рулевое управление", 1500));
            _availableParts.Add(new Part("Коробка передач", 4000));
            _availableParts.Add(new Part("Карбюратор", 800));
        }

        public Car CreateRandomCar()
        {
            var parts = new List<Part>();
            var isDamagedList = new List<bool>();

            int damagedCount = _random.Next(1, 4);

            foreach (var availablePart in _availableParts)
            {
                parts.Add(new Part(availablePart.Name, availablePart.Price));
            }

            for (int i = 0; i < parts.Count; i++)
            {
                isDamagedList.Add(false);
            }

            int damagedSoFar = 0;
            while (damagedSoFar < damagedCount)
            {
                int index = _random.Next(parts.Count);
                if (isDamagedList[index] == false)
                {
                    isDamagedList[index] = true;
                    damagedSoFar++;
                }
            }

            return new Car(parts, isDamagedList);
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