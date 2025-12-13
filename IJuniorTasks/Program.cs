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
        private const string CommandGetCards = "1";
        private const string CommandExit = "2";

        public void Run()
        {
            bool isWork = true;
            

            while (isWork)
            {
                Console.WriteLine($"\n\nВведите команду:");
                Console.WriteLine($"{CommandGetCards}. Раздать карты");
                Console.WriteLine($"{CommandExit}. Выход");
                Console.WriteLine();

                var command = Console.ReadLine();

                switch (command)
                {
                    case CommandGetCards:
                        //database.AddPlayer();
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

    public class Croupier
    {

    }

    public class Deck
    {
        private List<Card> Cards { get; set; }

        public Deck()
        {
            CreateFullDeck();
        }

        public void Shuffle()
        {
            var random = new Random();
            for (int i = 0; i < Cards.Count; i++)
            {
                var indexToChange = random.Next(Cards.Count);
                var temp = Cards[i];
                Cards[i] = Cards[indexToChange];
                Cards[indexToChange] = temp;
            }
        }

        public bool TryGetCards(int count, out List<Card> list)
        {
            list = new List<Card>();

            if (count > Cards.Count)
            {
                Console.WriteLine($"Недостаточно карт для раздачи {count} > {Cards.Count}");
                return false;
            }

            for (int i = 0; i < count; i++)
            {
                var card = Cards[i];
                list.Add(card);
                Cards.RemoveAt(i);
            }

            return true;
        }

        private void CreateFullDeck()
        {
            Cards = new List<Card>();
            foreach (var rank in Enum.GetValues<Ranks>())
            {
                foreach (var suit in Enum.GetValues<Suits>())
                {
                    var card = new Card(rank, suit);
                    Cards.Add(card);
                }
            }
        }
    }

    public class Card
    {
        public Card(Ranks rank, Suits suit)
        {
            Rank = rank;
            Suit = suit;
        }

        public Ranks Rank { get; }

        public Suits Suit { get; }
    }

    public enum Suits
    {
        // Пики 
        Spades = 1,

        // Червы  
        Hearts = 2,

        // Бубны  
        Diamonds = 3,

        // Трефы  
        Clubs = 4,
    }

    public enum Ranks
    {
        Ace = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 11,
        Queen = 12,
        King = 13
    }
}