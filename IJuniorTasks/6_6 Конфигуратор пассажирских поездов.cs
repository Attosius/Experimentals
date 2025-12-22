using System;
using System.Collections.Generic;
using System.Linq;

namespace IJuniorTasks66
{
    internal class Program66
    {
        static void Main66(string[] args)
        {
            var administrator = new Administrator();
            administrator.Run();
        }
    }

    public class Administrator
    {
        private const string CommandCreateTrain = "1";
        private const string CommandExit = "2";

        public void Run()
        {
            bool isWork = true;

            var dispatcher = new Dispatcher();

            while (isWork)
            {
                Console.Clear();
                Console.WriteLine($"Созданные поезда:");
                dispatcher.ShowTrains();

                Console.WriteLine($"\n\nВведите команду:");
                Console.WriteLine($"{CommandCreateTrain}. Создать поезд");
                Console.WriteLine($"{CommandExit}. Выход");
                Console.WriteLine();

                var command = Console.ReadLine();

                switch (command)
                {
                    case CommandCreateTrain:
                        dispatcher.CreateTrain();
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

    public class Dispatcher
    {
        private const int MinPassengersOnTrain = 200;
        private const int MaxPassengersOnTrain = 300;

        private readonly List<string> _stations = new() { "Москва", "СПб", "Казань", "Сочи", "Екатеринбург" };
        private readonly List<Train> _trains = new();

        public void CreateTrain()
        {
            Console.WriteLine($"Выберите пункт отправления:");

            if (!TryGetStation(out string stationDeparture))
            {
                Console.ReadLine();
                return;
            }

            Console.WriteLine($"Выберите пункт прибытия:");

            if (!TryGetStation(out string stationDestination))
            {
                Console.ReadLine();
                return;
            }

            if (stationDeparture.Equals(stationDestination, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Пункт отравления и пункт прибытия одинаковы!");
                Console.ReadLine();
                return;
            }

            var passengersCount = GetPassengers();
            var train = BuildTrain(stationDeparture, stationDestination, passengersCount);
            _trains.Add(train);
            Console.WriteLine($"Создан новый поезд {train.GetInfo()}");
            Console.ReadLine();
        }

        public void ShowTrains()
        {
            var index = 0;

            foreach (var train in _trains)
            {
                Console.WriteLine($"{++index}. {train.GetInfo()}");
            }
        }

        private Train BuildTrain(string stationDeparture, string stationDestination, int passengersCount)
        {
            var train = new Train(stationDeparture, stationDestination, passengersCount);

            while (passengersCount > 0)
            {
                var carriage = train.BuildCarriage();
                train.AddCarriage(carriage);
                passengersCount -= carriage.MaxPassengersCount;
            }

            return train;
        }

        private int GetPassengers()
        {
            Console.WriteLine("Продаем билеты...");
            var random = new Random();
            var passengers = random.Next(MinPassengersOnTrain, MaxPassengersOnTrain + 1);
            Console.WriteLine($"Продано билетов: {passengers}");
            return passengers;
        }

        private bool TryGetStation(out string station)
        {
            station = default;
            ShowStations();
            var stationIndexStr = Console.ReadLine();

            if (!int.TryParse(stationIndexStr, out var stationIndex) || stationIndex < 0 || stationIndex > _stations.Count)
            {
                Console.WriteLine($"Некорректный индекс!");
                return false;
            }

            station = _stations[stationIndex - 1];
            return true;
        }

        private void ShowStations()
        {
            var index = 0;

            foreach (var station in _stations)
            {
                Console.WriteLine($"{++index}. {station}");
            }
        }
    }

    public class Train
    {
        public const int BusinessClassFrequency = 5;

        private readonly List<Carriage> _carriages = new();

        public Train(string stationDeparture, string stationDestination, int passengersCount)
        {
            StationDeparture = stationDeparture;
            StationDestination = stationDestination;
            PassengersCount = passengersCount;
        }

        public string StationDeparture { get; }
        public string StationDestination { get; }
        public int PassengersCount { get; }
        public int MaxPassengersCount
        {
            get
            {
                return _carriages.Sum(carriage => carriage.MaxPassengersCount);
            }
        }
        public int CarriageCount => _carriages.Count;

        public void AddCarriage(Carriage carriage)
        {
            _carriages.Add(carriage);
        }

        public Carriage BuildCarriage()
        {
            var isCreateBusiness = CarriageCount + 1 % BusinessClassFrequency == 0;
            var carriagePassengersCount = isCreateBusiness
                ? Carriage.BusinessClassCountPassengers
                : Carriage.EconomyClassCountPassengers;
            var carriage = new Carriage(carriagePassengersCount);
            return carriage;
        }

        public string GetInfo()
        {
            return $"{StationDeparture} - {StationDestination}, пассажиров: {PassengersCount} / {MaxPassengersCount}, вагонов: {_carriages.Count}";
        }
    }

    public class Carriage
    {
        public const int EconomyClassCountPassengers = 50;
        public const int BusinessClassCountPassengers = 20;

        public Carriage(int maxPassengersCount)
        {
            MaxPassengersCount = maxPassengersCount;
        }

        public int MaxPassengersCount { get; }
    }
}