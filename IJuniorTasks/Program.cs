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
        AddCar = 1,
        ShowQueue,
        RepairCar,
        ShowWarehouse,
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
                autoService.ShowStats();

                Console.WriteLine($"\n\nВведите команду:");
                Console.WriteLine($"{(int)Commands.AddCar}. Добавить машину в очередь");
                Console.WriteLine($"{(int)Commands.ShowQueue}. Показать очередь машин");
                Console.WriteLine($"{(int)Commands.RepairCar}. Ремонтировать машину");
                Console.WriteLine($"{(int)Commands.ShowWarehouse}. Показать склад деталей");
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
                    case Commands.AddCar:
                        autoService.AddCar();
                        break;

                    case Commands.ShowQueue:
                        autoService.ShowQueue();
                        break;

                    case Commands.RepairCar:
                        autoService.RepairCar();
                        break;

                    case Commands.ShowWarehouse:
                        autoService.ShowWarehouse();
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
        private const int FixedPenalty = 500;
        private const int PenaltyPerBrokenPart = 200;

        private readonly Queue<Car> _carQueue = new();
        private readonly Warehouse _warehouse;
        private readonly CarFactory _carFactory;

        public AutoService()
        {
            _warehouse = new Warehouse();
            _carFactory = new CarFactory();
            InitializeWarehouse();
        }

        private void InitializeWarehouse()
        {
            _warehouse.AddPart(PartType.Engine, 5, 5000);
            _warehouse.AddPart(PartType.Transmission, 3, 3000);
            _warehouse.AddPart(PartType.Brakes, 10, 1000);
            _warehouse.AddPart(PartType.Suspension, 8, 1500);
            _warehouse.AddPart(PartType.Electronics, 6, 2000);
        }

        public decimal Balance { get; private set; } = 10000;

        public void ShowStats()
        {
            Console.WriteLine("=== АВТОСЕРВИС ===");
            Console.Write($"Баланс: ");
            UserUtils.WriteLine($"{Balance:C}", ConsoleColor.Green);
            Console.Write($"Машин в очереди: ");
            UserUtils.WriteLine($"{_carQueue.Count}", ConsoleColor.Cyan);
        }

        public void AddCar()
        {
            var car = _carFactory.CreateRandomCar();
            _carQueue.Enqueue(car);

            Console.Write($"Машина ");
            UserUtils.Write($"{car.Model}", ConsoleColor.Yellow);
            Console.WriteLine($" добавлена в очередь");
            Console.WriteLine($"Поломанных деталей: {car.GetBrokenPartsCount()}");
            Console.ReadKey();
        }

        public void ShowQueue()
        {
            if (_carQueue.Count == 0)
            {
                Console.WriteLine("Очередь пуста!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("=== ОЧЕРЕДЬ МАШИН ===");
            var index = 1;
            foreach (var car in _carQueue)
            {
                Console.Write($"{index}. ");
                UserUtils.Write($"{car.Model}", ConsoleColor.Yellow);
                Console.Write($" - поломок: ");
                UserUtils.WriteLine($"{car.GetBrokenPartsCount()}", ConsoleColor.Red);
                index++;
            }
            Console.ReadKey();
        }

        public void RepairCar()
        {
            if (_carQueue.Count == 0)
            {
                Console.WriteLine("Очередь пуста!");
                Console.ReadKey();
                return;
            }

            var car = _carQueue.Peek();
            RepairProcess(car);
        }

        private void RepairProcess(Car car)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"=== РЕМОНТ: {car.Model} ===");
                Console.Write($"Баланс: ");
                UserUtils.WriteLine($"{Balance:C}", ConsoleColor.Green);
                Console.WriteLine();

                car.ShowBrokenParts();

                Console.WriteLine($"\n\nДействия:");
                Console.WriteLine("1. Заменить деталь");
                Console.WriteLine("2. Отказаться от ремонта");
                Console.WriteLine();

                var choice = Console.ReadLine();

                if (choice == "1")
                {
                    if (TryReplacePart(car) == false)
                    {
                        continue;
                    }

                    if (car.IsFullyRepaired())
                    {
                        Console.WriteLine();
                        UserUtils.WriteLine("Машина полностью отремонтирована!", ConsoleColor.Green);
                        _carQueue.Dequeue();
                        Console.ReadKey();
                        return;
                    }
                }
                else if (choice == "2")
                {
                    RefuseRepair(car);
                    return;
                }
                else
                {
                    Console.WriteLine("Некорректный выбор!");
                    Console.ReadKey();
                }
            }
        }

        private bool TryReplacePart(Car car)
        {
            Console.WriteLine("\nВведите номер детали для замены:");
            var indexText = Console.ReadLine();

            if (int.TryParse(indexText, out var index) == false || index < 1 || index > car.Parts.Count)
            {
                Console.WriteLine("Некорректный номер!");
                Console.ReadKey();
                return false;
            }

            var part = car.Parts[index - 1];

            if (part.IsWorking)
            {
                Console.WriteLine("Эта деталь уже исправна!");
                Console.ReadKey();
                return false;
            }

            var partWithQuantity = _warehouse.GetPart(part.Type);

            if (partWithQuantity == null || partWithQuantity.Quantity == 0)
            {
                Console.WriteLine("Деталь отсутствует на складе!");
                Console.ReadKey();
                return false;
            }

            var repairPrice = partWithQuantity.Price + 500;
            Console.Write($"Стоимость ремонта: ");
            UserUtils.WriteLine($"{repairPrice:C}", ConsoleColor.Yellow);
            Console.WriteLine("Подтвердить замену? (д/н)");

            var confirm = Console.ReadLine()?.ToLower();

            if (confirm != "д" && confirm != "y")
            {
                Console.WriteLine("Замена отменена.");
                Console.ReadKey();
                return false;
            }

            _warehouse.RemovePart(part.Type);
            part.Fix();
            Balance += repairPrice;

            Console.Write($"Деталь ");
            UserUtils.Write($"{part.Type}", ConsoleColor.Cyan);
            Console.WriteLine(" заменена!");
            Console.ReadKey();
            return true;
        }

        private void RefuseRepair(Car car)
        {
            Console.Clear();
            Console.WriteLine($"=== ОТКАЗ ОТ РЕМОНТА: {car.Model} ===");

            decimal penalty;

            if (car.HasAnyRepairedParts())
            {
                penalty = car.GetBrokenPartsCount() * PenaltyPerBrokenPart;
                Console.WriteLine($"Отказ во время ремонта.");
                Console.Write($"Штраф за {car.GetBrokenPartsCount()} непочиненных деталей: ");
                UserUtils.WriteLine($"{penalty:C}", ConsoleColor.Red);
            }
            else
            {
                penalty = FixedPenalty;
                Console.WriteLine($"Отказ перед ремонтом.");
                Console.Write($"Фиксированный штраф: ");
                UserUtils.WriteLine($"{penalty:C}", ConsoleColor.Red);
            }

            Balance -= penalty;
            _carQueue.Dequeue();

            Console.Write($"Новый баланс: ");
            UserUtils.WriteLine($"{Balance:C}", ConsoleColor.Green);
            Console.ReadKey();
        }

        public void ShowWarehouse()
        {
            Console.WriteLine("=== СКЛАД ДЕТАЛЕЙ ===");
            _warehouse.ShowParts();
            Console.ReadKey();
        }

        public void ShowBalance()
        {
            Console.WriteLine("=== БАЛАНС ===");
            Console.Write($"Текущий баланс: ");
            UserUtils.WriteLine($"{Balance:C}", ConsoleColor.Green);
            Console.ReadKey();
        }
    }

    public class Car
    {
        public Car(string model, List<CarPart> parts)
        {
            Model = model;
            Parts = parts;
        }

        public string Model { get; }
        public List<CarPart> Parts { get; }

        public int GetBrokenPartsCount()
        {
            int count = 0;
            foreach (var part in Parts)
            {
                if (part.IsWorking == false)
                {
                    count++;
                }
            }
            return count;
        }

        public bool IsFullyRepaired()
        {
            foreach (var part in Parts)
            {
                if (part.IsWorking == false)
                {
                    return false;
                }
            }
            return true;
        }

        public bool HasAnyRepairedParts()
        {
            foreach (var part in Parts)
            {
                if (part.IsWorking)
                {
                    return true;
                }
            }
            return false;
        }

        public void ShowBrokenParts()
        {
            Console.WriteLine("Поломанные детали:");
            int index = 1;
            foreach (var part in Parts)
            {
                if (part.IsWorking == false)
                {
                    Console.Write($"{index}. ");
                    UserUtils.WriteLine($"{part.Type}", ConsoleColor.Red);
                    index++;
                }
            }
        }
    }

    public class CarPart
    {
        public CarPart(PartType type, bool isWorking)
        {
            Type = type;
            IsWorking = isWorking;
        }

        public PartType Type { get; }
        public bool IsWorking { get; private set; }

        public void Fix()
        {
            IsWorking = true;
        }
    }

    public enum PartType
    {
        Engine,
        Transmission,
        Brakes,
        Suspension,
        Electronics
    }

    public class PartWithQuantity
    {
        public PartWithQuantity(PartType type, int quantity, decimal price)
        {
            Type = type;
            Quantity = quantity;
            Price = price;
        }

        public PartType Type { get; }
        public int Quantity { get; set; }
        public decimal Price { get; }
    }

    public class Warehouse
    {
        private readonly List<PartWithQuantity> _parts = new();

        public void AddPart(PartType type, int quantity, decimal price)
        {
            var existingPart = _parts.Find(p => p.Type == type);

            if (existingPart != null)
            {
                existingPart.Quantity += quantity;
            }
            else
            {
                _parts.Add(new PartWithQuantity(type, quantity, price));
            }
        }

        public PartWithQuantity GetPart(PartType type)
        {
            return _parts.Find(p => p.Type == type);
        }

        public void RemovePart(PartType type)
        {
            var part = _parts.Find(p => p.Type == type);

            if (part != null && part.Quantity > 0)
            {
                part.Quantity--;
            }
        }

        public void ShowParts()
        {
            if (_parts.Count == 0)
            {
                Console.WriteLine("Склад пуст!");
                return;
            }

            foreach (var part in _parts)
            {
                Console.Write($"{part.Type}: ");
                UserUtils.Write($"{part.Quantity}", ConsoleColor.Cyan);
                Console.Write($" шт. - цена: ");
                UserUtils.WriteLine($"{part.Price:C}", ConsoleColor.Yellow);
            }
        }
    }

    public class CarFactory
    {
        private readonly Random _random = new();

        public Car CreateRandomCar()
        {
            var models = new[] { "Toyota Camry", "BMW X5", "Mercedes E-Class", "Audi A6", "Volkswagen Passat" };
            var model = models[_random.Next(models.Length)];

            var parts = new List<CarPart>
            {
                new CarPart(PartType.Engine, _random.NextDouble() > 0.3),
                new CarPart(PartType.Transmission, _random.NextDouble() > 0.4),
                new CarPart(PartType.Brakes, _random.NextDouble() > 0.5),
                new CarPart(PartType.Suspension, _random.NextDouble() > 0.4),
                new CarPart(PartType.Electronics, _random.NextDouble() > 0.3)
            };

            // Ensure at least one broken part
            var allWorking = true;
            foreach (var part in parts)
            {
                if (part.IsWorking == false)
                {
                    allWorking = false;
                    break;
                }
            }

            if (allWorking)
            {
                parts[_random.Next(parts.Count)] = new CarPart(parts[_random.Next(parts.Count)].Type, false);
            }

            return new Car(model, parts);
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
