using System;
using System.Collections.Generic;
using System.Linq;

namespace IJuniorTasks68
{
    internal class Program68
    {
        static void Main68(string[] args)
        {
            var administrator = new Administrator();
            administrator.Run();
        }
    }

    public class Administrator
    {
        private const string CommandServeClient = "1";
        private const string CommandExit = "2";

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

                supermarket.CheckStreet();
                Console.ReadKey();
                Console.Clear();
            }
        }
    }

    public class Supermarket
    {
        private const int InitalClientsCount = 4;

        private readonly List<Product> _products;
        private readonly ProductsFactory _productsFactory = new();
        private readonly Queue<Client> _clients = new();

        public Supermarket()
        {
            _products = _productsFactory.GetProducts();
            AddClients();
        }

        public int ClientsCount => _clients.Count;
        public decimal Income { get; private set; }

        public void CheckStreet()
        {
            if (_clients.Count == 0)
            {
                AddClients();
                Console.WriteLine($"Супермаркет: Набрали с улицы еще {InitalClientsCount} клиентов");
            }
        }

        public void ServeClient()
        {
            var client = GetClient();

            var totalAmount = ProcessCalculating(client);

            if (client.ProductsCount == 0)
            {
                Console.WriteLine($"{client.Name}: Ничего не купил, ушел злой");
                Console.WriteLine($"Супермаркет: на счету все также {Income} руб.");
                return;
            }

            ProcessPayment(client, totalAmount);
        }

        private Client GetClient()
        {
            var client = _clients.Dequeue();
            client.FillBasket(_products);
            Console.WriteLine($"{client.Name}: Финансы: {client.Money} руб. корзина состоит из {client.ProductsCount} продуктов");
            return client;
        }

        private decimal ProcessCalculating(Client client)
        {
            var canPay = false;
            decimal totalAmount = 0;

            while (canPay == false && client.ProductsCount > 0)
            {
                totalAmount = client.CalculateBasket();
                Console.WriteLine($"{client.Name}: Стоимость корзины {totalAmount} руб.");
                canPay = client.HasMoneyToPay(totalAmount);

                if (canPay == false)
                {
                    var product = client.RemoveRandomProduct();
                    Console.WriteLine($"{client.Name}: Денег не хватает, убираем {product.Name} за {product.Price} руб.");
                }
            }

            return totalAmount;
        }

        private void ProcessPayment(Client client, decimal totalAmount)
        {
            client.BuyBasket(totalAmount);
            Income += totalAmount;
            Console.WriteLine($"{client.Name}: Финансы: {client.Money} руб., купил товаров: {client.BagCount}, стоимость покупки {totalAmount} руб.");
            Console.WriteLine($"Супермаркет: на счету {Income} руб.");
        }

        private void AddClients()
        {
            for (int i = 0; i < InitalClientsCount; i++)
            {
                _clients.Enqueue(ClientsFactory.GetNewClient());
            }
        }
    }

    public class Client
    {
        private const int MaxProductsInBasket = 5;

        private readonly List<Product> _basket = new();
        private List<Product> _bag = new();

        public Client(string name, decimal money)
        {
            Name = name;
            Money = money;
        }

        public string Name { get; }
        public decimal Money { get; private set; }
        public int BagCount => _bag.Count;
        public int ProductsCount => _basket.Count;

        public void FillBasket(List<Product> availableProducts)
        {
            var productsCount = UserUtils.GenerateRandomNumber(1, MaxProductsInBasket);

            for (int i = 0; i < productsCount; i++)
            {
                var productIndex = UserUtils.GenerateRandomNumber(availableProducts.Count - 1);
                _basket.Add(availableProducts[productIndex]);
            }
        }

        public decimal CalculateBasket()
        {
            return _basket.Sum(o => o.Price);
        }

        public bool HasMoneyToPay(decimal totalAmount)
        {
            return Money >= totalAmount;
        }

        public Product RemoveRandomProduct()
        {
            var randomProductIndex = UserUtils.GenerateRandomNumber(_basket.Count - 1);
            var productToRemove = _basket[randomProductIndex];
            _basket.Remove(productToRemove);
            return productToRemove;
        }

        public void BuyBasket(decimal totalAmount)
        {
            Money -= totalAmount;
            _bag = _basket.ToList();
            _basket.Clear();
        }
    }

    public class Product
    {
        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }
        public string Name { get; }
        public decimal Price { get; }

    }

    public class ClientsFactory
    {
        private const decimal MinClientMoney = 20;
        private const decimal MaxClientMoney = 100;

        private static int s_clientsCount;

        public static Client GetNewClient()
        {
            var clientMoney = UserUtils.GenerateRandomNumber((int)MinClientMoney, (int)MaxClientMoney);
            s_clientsCount++;
            return new Client($"Клиент #{s_clientsCount}", clientMoney);
        }
    }

    public class ProductsFactory
    {
        private readonly List<Product> s_products = new();

        public ProductsFactory()
        {
            s_products.Add(new Product("Хлеб", 10));
            s_products.Add(new Product("Мясо", 50));
            s_products.Add(new Product("Молоко", 20));
            s_products.Add(new Product("Шоколад", 50));
            s_products.Add(new Product("Кофе", 60));
            s_products.Add(new Product("Коньяк", 80));
        }

        public List<Product> GetProducts()
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
    }
}