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


//Константы
//    Поля 
//Конструкторы 
//    Свойства
//Методы
//    Каждая категория сортируется по публичности:
//public
//    protected
//    private



//При написании кода стоит применять все правила, описанные в этом блоке и в предыдущих по требованиям к задачам.

//- Поле/свойство названо в соответствии с нотацией.
//Имя полей/свойств должны быть названы в соответствии с нотацией.
// Поля public - с большой буквы, private - с _ и маленькой буквы, protected - с большой.
//    Свойства, методы всегда с большой буквы.
//    Константы всегда с большой буквы.
//    Приватные поля со static именуются так: s_name - где name - уже данное вами имя полю.

//- Очередность в классе. Сначала приватные поля, пустая строка, конструктор, пустая строка, свойства, пустая строка, публичные и приватные методы. Между методами пустая строка. Чтобы подробнее рассмотреть очередность в классе посмотрите на https://clck.ru/at8vs

//-Отсутствие повторения имени класса внутри класса. Имена полей/свойств/методов не должны содержать имя класса, в котором они находятся.

//- Соблюдена необходимая доступность полей класс. Отсутствуют публичные поля и вне класса не передаются ссылочные типы, которые должны изменяться только внутри класса.

//- Выделены все возможные классы. Класс - это набор данных и методов для оперирования с этими данными. 