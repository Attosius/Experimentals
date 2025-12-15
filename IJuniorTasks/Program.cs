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
        private const string CommandShowGoods = "1";
        private const string CommandBuy = "2";
        private const string CommandShowPurchases = "3";
        private const string CommandExit = "4";

        public void Run()
        {
            bool isWork = true;

            var store = new Dispatcher();

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

    public class Dispatcher
    {

    }
    
}