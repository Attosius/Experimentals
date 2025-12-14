using System;
using System.Collections.Generic;

namespace IJuniorTasks63
{
    internal class Program63
    {
        static void Main63(string[] args)
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
            var croupier = new Croupier();

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
                        croupier.GetCards();
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
        private readonly Player _player;
        private readonly Deck _deck;

        public Croupier()
        {
            _player = new Player();
            _deck = new Deck(false);
            _deck.Shuffle();
        }

        public void GetCards()
        {
            Console.WriteLine("Введите количество карт для раздачи:");
            var countString = Console.ReadLine();

            if (!int.TryParse(countString, out var countCards))
            {
                Console.WriteLine("Некорректное число");
                return;
            }

            if (!_deck.TryGetCards(countCards, out var cards))
            {
                return;
            }

            _player.AddCards(cards);
            Console.WriteLine($"Успешно добавлено карт игроку: {countCards}");
            Console.WriteLine($"Крупье: {_deck.Count} карт");
            Console.WriteLine($"Игрок: {_player.CardsCount} карт");
            Console.WriteLine("Карты игрока:");
            _player.GetInfo();
        }
    }

    public class Player
    {
        private readonly Deck _deck;

        public Player()
        {
            _deck = new Deck();
        }

        public int CardsCount => _deck.Count;

        public void AddCards(List<Card> cards)
        {
            _deck.AddCards(cards);
        }

        public void GetInfo()
        {
            _deck.GetInfo();
        }
    }

    public class Deck
    {
        private List<Card> _cards = new();

        public Deck(bool isEmpty = true)
        {
            if (isEmpty == false)
            {
                Fill();
            }
        }

        public int Count => _cards.Count;

        public void Shuffle()
        {
            var random = new Random();
            for (int i = 0; i < _cards.Count; i++)
            {
                var indexToChange = random.Next(_cards.Count);
                var temp = _cards[i];
                _cards[i] = _cards[indexToChange];
                _cards[indexToChange] = temp;
            }
        }

        public bool TryGetCards(int count, out List<Card> list)
        {
            list = new List<Card>();

            if (count > _cards.Count)
            {
                Console.WriteLine($"Недостаточно карт для раздачи {count} > {_cards.Count}");
                return false;
            }

            for (int i = 0; i < count; i++)
            {
                var card = _cards[i];
                list.Add(card);
                _cards.RemoveAt(i);
            }

            return true;
        }

        public void AddCards(List<Card> cards)
        {
            _cards.AddRange(cards);
        }

        private void Fill()
        {
            _cards = new List<Card>();
            foreach (var rank in Enum.GetValues<Ranks>())
            {
                foreach (var suit in Enum.GetValues<Suits>())
                {
                    var card = new Card(rank, suit);
                    _cards.Add(card);
                }
            }
        }

        public void GetInfo()
        {
            var index = 0;
            foreach (var card in _cards)
            {
                Console.Write($"{++index}. {card}\n");
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

        public override string ToString()
        {
            return $"Ранг: {Rank},\t Масть: {Suit}";
        }
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