using System;
using System.Collections.Generic;
using System.Linq;

namespace IJuniorTasks65
{
    internal class Program65
    {
        static void Main65(string[] args)
        {
            var administrator = new Administrator();
            administrator.Run();
        }
    }

    public class Administrator
    {
        private const string CommandShowGoods = "1";
        private const string CommandBuy = "2";
        private const string CommandShowPurchases = "3";
        private const string CommandExit = "4";

        public void Run()
        {
            bool isWork = true;

            var store = new Store();

            while (isWork)
            {
                Console.WriteLine($"\n\nВведите команду:");
                Console.WriteLine($"{CommandShowGoods}. Показать товары");
                Console.WriteLine($"{CommandBuy}. Купить товары");
                Console.WriteLine($"{CommandShowPurchases}. Показать купленные товары");
                Console.WriteLine($"{CommandExit}. Выход");
                Console.WriteLine();

                var command = Console.ReadLine();

                switch (command)
                {
                    case CommandShowGoods:
                        store.ShowSellersItems();
                        break;

                    case CommandBuy:
                        store.SellItem();
                        break;

                    case CommandShowPurchases:
                        store.ShowBuyerInfo();
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

    public class Store
    {
        private readonly Seller _seller;
        private readonly Buyer _buyer;

        public Store()
        {
            _seller = new Seller();
            _buyer = new Buyer();
        }

        public void ShowSellersItems()
        {
            Console.WriteLine($"Товары продавца");
            _seller.ShowItems();
        }

        public void SellItem()
        {
            ShowSellersItems();
            Console.WriteLine($"Введите Id товара для покупки:");
            var idString = Console.ReadLine();

            if (int.TryParse(idString, out var id) == false)
            {
                Console.WriteLine("Некорректный Id");
                return;
            }

            if (_seller.TryGetItemById(id, out var item) == false)
            {
                Console.WriteLine("Не удалось найти товар");
                return;
            }

            if (_buyer.TryBuyItem(item) == false)
            {
                Console.WriteLine("Неудалось совершить покупку");
                return;
            }

            _seller.SellItem(item);
            Console.WriteLine($"Покупка успешно совершена: {item}");
        }

        public void ShowBuyerInfo()
        {
            _buyer.ShowMoney();
            Console.WriteLine($"Товары у покупателя");
            _buyer.ShowItems();
        }
    }

    public class Seller : Person
    {
        private int _maxItemId = 0;

        public Seller()
        {
            Fill();
        }

        public void Fill()
        {
            Items.Add(new Item(++_maxItemId, "Ржавый меч", 5));
            Items.Add(new Item(++_maxItemId, "Меч", 10));
            Items.Add(new Item(++_maxItemId, "Длинный меч", 20));
            Items.Add(new Item(++_maxItemId, "Зелье здоровья", 10));
            Items.Add(new Item(++_maxItemId, "Зелье маны", 10));
        }

        public bool TryGetItemById(int id, out Item item)
        {
            item = Items.FirstOrDefault(item => item.Id == id);
            return item != null;
        }

        public void SellItem(Item item)
        {
            Items.Remove(item);
            Money += item.Cost;
        }
    }

    public class Buyer : Person
    {
        private static readonly decimal s_defaultMoney = 20;

        public Buyer()
        {
            Money = s_defaultMoney;
        }

        public void ShowMoney()
        {
            Console.WriteLine($"Количество денег у покупателя: {Money}");
        }

        public bool TryBuyItem(Item item)
        {
            if (item.Cost > Money)
            {
                Console.WriteLine($"Недостаточно денег");
                return false;
            }

            Items.Add(item);
            Money -= item.Cost;
            return true;
        }
    }

    public class Person
    {
        protected List<Item> Items = new();

        public int ItemsCount => Items.Count;

        public decimal Money { get; protected set; }

        public void ShowItems()
        {
            if (Items.Count == 0)
            {
                Console.WriteLine($"Пусто");
                return;
            }

            var index = 0;

            foreach (var item in Items)
            {
                index++;
                Console.WriteLine($"{index}. {item}");
            }
        }

    }

    public class Item
    {
        public Item(int id, string name, decimal cost)
        {
            Id = id;
            Name = name;
            Cost = cost;
        }

        public int Id { get; }
        public string Name { get; }
        public decimal Cost { get; }

        public override string ToString()
        {
            return $"Id: {Id}, {Name}, {Cost}$";
        }
    }
}