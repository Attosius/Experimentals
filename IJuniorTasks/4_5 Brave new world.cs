using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace IJuniorTasks
{
    internal class Program45
    {
        static void Main45(string[] args)
        {
            const char PersonSymbol = '@';
            const char BonusSymbol = '$';
            const char WallSymbol = '#';
            const char ScoreSymbol = '.';
            const char EmptySymbol = ' ';

            int mapWidth = 20;
            int mapHeight = 10;
            int personPositionX = 3;
            int personPositionY = 4;

            int scoreSymbolCost = 1;
            int bonusSymbolCost = 10;

            int score = 0;

            var map = ReadMapFromFile(mapHeight, mapWidth);
            var maxScore = CalculateMaxScore(map, ScoreSymbol, BonusSymbol, scoreSymbolCost, bonusSymbolCost);

            bool isWork = true;
            Console.WriteLine($"Press space to begin");

            while (isWork)
            {
                var command = Console.ReadKey(true);
                var exitCommand = ConsoleKey.Q;

                if (IsQuit(command, exitCommand))
                {
                    isWork = false;
                    continue;
                }

                int personNextPositionX = personPositionX;
                int personNextPositionY = personPositionY;
                HandleInput(command, ref personNextPositionY, ref personNextPositionX);

                if (TryMovePerson(map, ref personPositionX, ref personPositionY, personNextPositionY, personNextPositionX,
                        PersonSymbol, WallSymbol, ScoreSymbol, BonusSymbol, EmptySymbol, bonusSymbolCost, scoreSymbolCost, ref score) == false)
                {
                    continue;
                }

                ShowMap(map);
                Console.WriteLine($"Person position ({personPositionX}, {personPositionY})");
                Console.WriteLine($"Score:  {score}");
                Console.WriteLine($"{exitCommand} to exit");

                if (IsWin(score, maxScore))
                {
                    isWork = false;
                }
            }
        }

        private static bool IsQuit(ConsoleKeyInfo command, ConsoleKey exitCommand)
        {
            return command.Key == exitCommand;
        }

        private static char[,] ReadMapFromFile(int mapHeight, int mapWidth)
        {
            const string mapFileName = "map.txt";
            var file = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), mapFileName));

            char[,] map = new char[mapHeight, mapWidth];

            for (int i = 0; i < mapHeight; i++)
            {
                for (int j = 0; j < mapWidth; j++)
                {
                    map[i, j] = file[i][j];
                }
            }

            return map;
        }

        private static void ShowMap(char[,] map)
        {
            Console.SetCursorPosition(0, 0);

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Console.Write(map[i, j]);
                }

                Console.WriteLine();
            }
        }

        private static bool TryMovePerson(char[,] map,
            ref int personPositionX,
            ref int personPositionY,
            int personNextPositionY,
            int personNextPositionX,
            char personSymbol,
            char wallSymbol,
            char scoreSymbol,
            char bonusSymbol,
            char emptySymbol,
            int bonusSymbolCost,
            int scoreSymbolCost,
            ref int score)
        {
            var personNextPositionTile = map[personNextPositionY, personNextPositionX];

            if (personNextPositionTile == wallSymbol)
            {
                return false;
            }
            else if (personNextPositionTile == bonusSymbol)
            {
                score += bonusSymbolCost;
            }
            else if (personNextPositionTile == scoreSymbol)
            {
                score += scoreSymbolCost;
            }

            map[personPositionY, personPositionX] = emptySymbol;
            personPositionX = personNextPositionX;
            personPositionY = personNextPositionY;
            SetPerson(map, personPositionX, personPositionY, personSymbol);
            return true;
        }

        private static void HandleInput(ConsoleKeyInfo command, ref int personNextPositionY, ref int personNextPositionX)
        {
            const ConsoleKey CommandUp = ConsoleKey.UpArrow;
            const ConsoleKey CommandUpAlternative = ConsoleKey.W;

            const ConsoleKey CommandDown = ConsoleKey.DownArrow;
            const ConsoleKey CommandDownAlternative = ConsoleKey.S;

            const ConsoleKey CommandLeft = ConsoleKey.LeftArrow;
            const ConsoleKey CommandLeftAlternative = ConsoleKey.A;

            const ConsoleKey CommandRight = ConsoleKey.RightArrow;
            const ConsoleKey CommandRightAlternative = ConsoleKey.D;

            switch (command.Key)
            {
                case CommandUp:
                case CommandUpAlternative:
                    personNextPositionY--;
                    break;

                case CommandDown:
                case CommandDownAlternative:
                    personNextPositionY++;
                    break;

                case CommandLeft:
                case CommandLeftAlternative:
                    personNextPositionX--;
                    break;

                case CommandRight:
                case CommandRightAlternative:
                    personNextPositionX++;
                    break;
            }
        }

        private static int CalculateMaxScore(char[,] map, char scoreSymbol, char bonusSymbol, int scoreSymbolCost, int bonusSymbolCost)
        {
            var score = 0;

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    var symbol = map[i, j];

                    if (symbol == bonusSymbol)
                    {
                        score += bonusSymbolCost;
                    }
                    else if (symbol == scoreSymbol)
                    {
                        score += scoreSymbolCost;
                    }
                }
            }

            return score;
        }

        private static void SetPerson(char[,] map, int personPositionX, int personPositionY, char personSymbol)
        {
            map[personPositionY, personPositionX] = personSymbol;
        }

        private static bool IsWin(int score, int maxScore)
        {
            if (score >= maxScore)
            {
                Console.WriteLine($"You win!");
                return true;
            }

            return false;
        }
    }
}