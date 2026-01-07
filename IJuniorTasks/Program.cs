using System;
using System.Collections.Generic;
using System.Linq;

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
        private const string CommandServeClient = "1";
        private const string CommandExit = "4";

        public void Run()
        {
            bool isWork = true;

            var supermarket = new Supermarket();

            while (isWork)
            {
                Console.WriteLine($"В очереди {supermarket.ClientsCount} клиентов");
                Console.WriteLine($"Заработано {supermarket.Income} pyб.");

                Console.WriteLine($"\n\nВведите команду:");
                Console.WriteLine($"{CommandServeClient}. Обслужить клиента");
                Console.WriteLine($"{CommandExit}. Выход");
                Console.WriteLine();

                var command = Console.ReadLine();

                switch (command)
                {
                    case CommandServeClient:
                        supermarket.ServeClient();
                        break;
                        
                    case CommandExit:
                        isWork = false;
                        break;

                    default:
                        Console.WriteLine($"Некорректная команда!");
                        break;
                }

                Console.ReadKey();
                Console.Clear();
            }
        }
    }

    public class Supermarket
    {
        private const int InitalClientsCount = 10;
        

        private readonly List<Product> _products;
        private readonly Queue<Client> _clients = new Queue<Client>();

        public int ClientsCount => _clients.Count;
        public int Income { get; private set}

        public Supermarket()
        {
            _products = ProductsHub.GetProducts();
            AddClients();
        }
        
        public void ServeClient()
        {
            var client = _clients.Dequeue();
            client.FillBasket(_products);

            for (int i = 0; i < client.; i++)
            {
                
            }
        }

        private void AddClients()
        {
            for (int i = 0; i < InitalClientsCount; i++)
            {
                _clients.Enqueue(ClientsHub.GetNewClient());
            }
        }

    }

    public class Client
    {
        private const int MaxProductsInBasket = 5;
        public decimal Money { get; private set; }

        private List<Product> _basketProducts = new List<Product>();
        private List<Product> _bag = new List<Product>();


        public Client(decimal money)
        {
            Money = money;
        }

        public void FillBasket(List<Product> availableProducts)
        {
            var productsCount = UserUtils.GenerateRandomNumber(MaxProductsInBasket);

            for (int i = 0; i < productsCount; i++)
            {
                var productIndex = UserUtils.GenerateRandomNumber(availableProducts.Count);
                _basketProducts.Add(availableProducts[productIndex]);
            }
        }
    }

    public class Product
    {
        public string Name { get; }
        public decimal Price { get; }

        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }
    }

    public static class ClientsHub
    {
        private const decimal MinClientMoney = 20;
        private const decimal MaxClientMoney = 100;

        public static Client GetNewClient()
        {
            var clientMoney = UserUtils.GenerateRandomNumber((int)MinClientMoney, (int)MaxClientMoney);
            return new Client(clientMoney);
        }
    }

    public static class ProductsHub
    {
        private static readonly List<Product> s_products = new ();

        static ProductsHub()
        {
            s_products.Add(new Product("Хлеб", 10));
            s_products.Add(new Product("Мясо", 50));
            s_products.Add(new Product("Молоко", 20));
            s_products.Add(new Product("Шоколад", 50));
            s_products.Add(new Product("Кофе", 60));
            s_products.Add(new Product("Коньяк", 80));
        }

        public static List<Product> GetProducts()
        {
            return s_products.ToList();
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