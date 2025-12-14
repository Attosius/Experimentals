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
        private const string CommandAddBook = "1";
        private const string CommandRemoveBook = "2";
        private const string CommandShow = "3";
        private const string CommandFind = "4";
        private const string CommandExit = "5";

        public void Run()
        {
            bool isWork = true;

            var database = new DatabaseBooks();

            while (isWork)
            {
                Console.WriteLine($"\n\nВведите команду:");
                Console.WriteLine($"{CommandAddBook}. Добавить книгу");
                Console.WriteLine($"{CommandRemoveBook}. Удалить книгу");
                Console.WriteLine($"{CommandShow}. Показать книги");
                Console.WriteLine($"{CommandFind}. Найти книги");
                Console.WriteLine($"{CommandExit}. Выход");
                Console.WriteLine();

                var command = Console.ReadLine();

                switch (command)
                {
                    case CommandAddBook:
                        database.AddBook();
                        break;

                    case CommandRemoveBook:
                        database.RemoveBook();
                        break;

                    case CommandShow:
                        database.ShowAllBooks();
                        break;

                    case CommandFind:
                        database.FindBook();
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

    public class DatabaseBooks
    {
        private const string CommandFindByName = "1";
        private const string CommandFindByAuthor = "2";
        private const string CommandFindByYear = "3";

        private readonly List<Book> _booksList = new();
        private int _booksCount = 0;

        public void AddBook()
        {
            Console.WriteLine($"Введите название книги:");
            var name = Console.ReadLine();

            Console.WriteLine($"Введите автора книги:");
            var author = Console.ReadLine();

            Console.WriteLine($"Введите год выпуска книги:");
            var yearString = Console.ReadLine();

            if (int.TryParse(yearString, out var year) == false || year < 0 || year > DateTime.Today.Year)
            {
                Console.WriteLine($"Некорректный год.");
                return;
            }

            var id = GetNewId();

            var book = new Book(id, name, author, year);
            _booksList.Add(book);
            Console.WriteLine($"Книга {name} успешно добавлена. Id: {id}");
        }

        public void RemoveBook()
        {
            if (TryGetBookById(out var book) == false)
            {
                return;
            }

            _booksList.Remove(book);
            Console.WriteLine($"Книга c Id {book.Id} успешно удалена");
        }
        
        public void FindBook()
        {
            Console.WriteLine($"Выберите критерий для поиска:");
            Console.WriteLine($"{CommandFindByName}. По названию");
            Console.WriteLine($"{CommandFindByAuthor}. По автору");
            Console.WriteLine($"{CommandFindByYear}. По году выпуска");

            var criteriaString = Console.ReadLine();
            var books = new List<Book>();

            switch (criteriaString)
            {
                case CommandFindByName:
                    books = FindBooksByName();
                    break;

                case CommandFindByAuthor:
                    books = FindBooksByAuthor();
                    break;

                case CommandFindByYear:
                    books = FindBooksByYear();
                    break;

                default:
                    Console.WriteLine($"Некорректная команда!");
                    return;
            }

            if (books.Count == 0)
            {
                Console.WriteLine("Книги по заданному критерию не найдены");
                return;
            }

            ShowBooks(books);
        }

        public void ShowAllBooks()
        {
            ShowBooks(_booksList);
        }

        private void ShowBooks(List<Book> books)
        {
            var index = 0;

            foreach (var player in books)
            {
                index++;
                Console.WriteLine($"{index}. {player}");
            }
        }

        private int GetNewId()
        {
            return ++_booksCount;
        }

        private bool TryGetBookById(out Book book)
        {
            book = default;
            Console.WriteLine("Введите id книги:");
            var idString = Console.ReadLine();

            if (int.TryParse(idString, out var id) == false)
            {
                Console.WriteLine($"Некорректный Id: {id}");
                return false;
            }

            book = _booksList.FirstOrDefault(book => book.Id == id);

            if (book == null)
            {
                Console.WriteLine($"Книга c Id {id} не найден");
                return false;
            }

            return true;
        }

        private List<Book> FindBooksByName()
        {
            Console.WriteLine($"Введите название книги для поиска:");
            var name = Console.ReadLine();
            var books = _booksList
                .Where(book => book.Name.Contains(name))
                .ToList();

            return books;
        }

        private List<Book> FindBooksByAuthor()
        {
            Console.WriteLine($"Введите автора книги для поиска:");
            var author = Console.ReadLine();
            var books = _booksList
                .Where(book => book.Author.Contains(author))
                .ToList();

            return books;
        }

        private List<Book> FindBooksByYear()
        {
            Console.WriteLine($"Введите год выпуска книги для поиска:");
            var yearString = Console.ReadLine();

            if (int.TryParse(yearString, out var year) == false)
            {
                Console.WriteLine($"Некорректный год.");
                return new List<Book>();
            }

            var books = _booksList
                .Where(book => book.Year == year)
                .ToList();

            return books;
        }
    }

    public class Book
    {
        public Book(int id, string name, string author, int year)
        {
            Id = id;
            Name = name;
            Author = author;
            Year = year;
        }
        
        public int Id { get; }
        public string Name { get; }
        public string Author { get; }
        public int Year { get; }

        public override string ToString()
        {
            return $"Id: {Id},\t Name: {Name.PadRight(15)},\t Author: {Author.PadRight(15)},\t Year: {Year}";
        }
    }
}