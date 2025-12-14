using System;

namespace IJuniorTasks61
{
    internal class Program62
    {
        static void Main62(string[] args)
        {
            var player = new Player(10, 20, "@");
            var drawer = new Drawer();
            drawer.DrawPlayer(player);
            Console.ReadKey();
        }
    }

    public class Player
    {
        public Player(int positionX, int positionY, string symbol)
        {
            PositionX = positionX;
            PositionY = positionY;
            Symbol = symbol;
        }

        public int PositionX { get; }
        public int PositionY { get; }
        public string Symbol { get; }
    }
    
    public class Drawer
    {
        public void DrawPlayer(Player player)
        {
            var consolePosition = Console.GetCursorPosition();
            Console.SetCursorPosition(player.PositionX, player.PositionY);
            Console.Write(player.Symbol);
            Console.SetCursorPosition(consolePosition.Left, consolePosition.Top);
        }
    }
}