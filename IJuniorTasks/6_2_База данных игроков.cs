using System;
using System.Collections.Generic;
using System.Linq;

namespace IJuniorTasks62
{
    internal class Program
    {
        static void Main62(string[] args)
        {
            var administrator = new Administrator();
            administrator.Run();
        }
    }

    public class Administrator
    {
        private const string CommandAddPlayer = "1";
        private const string CommandRemovePlayer = "2";
        private const string CommandBan = "3";
        private const string CommandUnban = "4";
        private const string CommandShow = "5";
        private const string CommandExit = "6";

        public void Run()
        {
            bool isWork = true;

            var database = new DatabasePlayers();

            while (isWork)
            {
                Console.WriteLine($"\n\nВведите команду:");
                Console.WriteLine($"{CommandAddPlayer}. Добавить игрока");
                Console.WriteLine($"{CommandRemovePlayer}. Удалить игрока");
                Console.WriteLine($"{CommandBan}. Забанить игрока");
                Console.WriteLine($"{CommandUnban}. Разбанить игрока");
                Console.WriteLine($"{CommandShow}. Показать игроков");
                Console.WriteLine($"{CommandExit}. Выход");
                Console.WriteLine();

                var command = Console.ReadLine();

                switch (command)
                {
                    case CommandAddPlayer:
                        database.AddPlayer();
                        break;

                    case CommandRemovePlayer:
                        database.RemovePlayer();
                        break;

                    case CommandBan:
                        database.BanPlayer();
                        break;

                    case CommandUnban:
                        database.UnbanPlayer();
                        break;

                    case CommandShow:
                        database.ShowAll();
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

    public class DatabasePlayers
    {
        private readonly List<Player> _playerList = new();
        private int _playersCount = 0;

        public void AddPlayer()
        {
            Console.WriteLine($"Введите ник игрока:");
            var nick = Console.ReadLine();
            var id = GetNewId();
            var player = new Player(id, nick, Player.DefaultLevel);

            _playerList.Add(player);
            Console.WriteLine($"Игрок {nick} успешно добавлен. Id: {id}");
        }

        public void RemovePlayer()
        {
            if (!TryGetPlayerById(out var player))
            {
                return;
            }

            _playerList.Remove(player);
            Console.WriteLine($"Игрок c Id {player.Id} успешно удален");
        }

        public void BanPlayer()
        {
            if (!TryGetPlayerById(out var player))
            {
                return;
            }

            player.BanPlayer();
            Console.WriteLine($"Игрок c Id {player.Id} успешно забанен");
        }

        public void UnbanPlayer()
        {
            if (!TryGetPlayerById(out var player))
            {
                return;
            }

            player.UnBanPlayer();
            Console.WriteLine($"Игрок c Id {player.Id} успешно разбанен");
        }

        public void ShowAll()
        {
            var index = 0;

            foreach (var player in _playerList)
            {
                index++;
                Console.WriteLine($"{index}. {player}");
            }
        }

        private int GetNewId()
        {
            return ++_playersCount;
        }

        private bool TryGetPlayerById(out Player player)
        {
            player = default;
            Console.WriteLine("Введите id игрока:");
            var idString = Console.ReadLine();

            if (!int.TryParse(idString, out var id))
            {
                Console.WriteLine($"Некорректный Id: {id}");
                return false;
            }

            player = _playerList.FirstOrDefault(o => o.Id == id);

            if (player == null)
            {
                Console.WriteLine($"Игрок c Id {id} не найден");
                return false;
            }

            return true;
        }
    }

    public class Player
    {
        public Player(int id, string nick, int level)
        {
            Id = id;
            Nick = nick;
            Level = level;
        }

        public static int DefaultLevel = 0;

        public int Id { get; }
        public string Nick { get; }
        public int Level { get; }
        public bool IsBan { get; private set; }


        public void BanPlayer()
        {
            IsBan = true;
        }

        public void UnBanPlayer()
        {
            IsBan = false;
        }

        public override string ToString()
        {
            return $"Id: {Id},\t Nick: {Nick.PadRight(15)}, Level: {Level},\t IsBan: {IsBan}";
        }
    }
}