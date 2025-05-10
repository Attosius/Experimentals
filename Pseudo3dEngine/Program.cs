using System;
using System.Diagnostics;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Pseudo3dEngine
{
    public class Program
    {
        public static uint ScreenHeight = 1200;
        public static uint ScreenWidth = 800;

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Hello, World!");
                var videoMode = new VideoMode(ScreenHeight, ScreenWidth);
                var window = new RenderWindow(videoMode, "GL HF!");
                window.Display();

                var texture = new Texture(@"d:\Projects\Experimentals\Pseudo3dEngine\cold_heart.jpeg");
                //var sprite = new Sprite(texture);

                var font = new Font(@"d:\Projects\\Experimentals\Pseudo3dEngine\cour.ttf");

                //var circleShape = new CircleShape(50);
                //circleShape.FillColor = Color.Green;
                
                //var rectangle = new RectangleShape(new Vector2f(120f, 50f));
                //rectangle.FillColor = new Color(255, 175, 174);
                //rectangle.Size = new Vector2f(10, 10);
                //rectangle.Position = new Vector2f(0, 0);
                //rectangle.OutlineThickness = 2;
                //rectangle.OutlineColor = new Color(255, 255, 255);
                
                //var line = new RectangleShape (new Vector2f(10, 133));
                //line.Rotation = 45;


                //var lineArr = new Vertex[]
                //{
                //    new Vertex(new Vector2f(10, 50)),
                //    new Vertex(new Vector2f(150, 150)),
                //    new Vertex(new Vector2f(50, 10))
                //};

                var person = new Person();
                
                var sw = new Stopwatch();
                var world = new World();
                var mapCoordinates = new MapCoordinates(ScreenHeight, ScreenWidth);
                //var mousePosition = new MousePosition(window);
                // Start the game loop
                while (window.IsOpen)
                {
                    var elapsed = sw.ElapsedMilliseconds / (double)1000;
                    sw.Restart();
                    Thread.Sleep(10);

                    window.DispatchEvents();
                    window.Closed += (sender, _) => ((RenderWindow)sender!).Close();
                    
                    InputEvents(person, mapCoordinates);

                    window.Clear();
                    
                    //window.Draw(sprite);
                    
                    //window.Draw(circleShape);
                    //window.Draw(rectangle);
                    //window.Draw(line);
                    //window.Draw(cam);
                    //window.Draw(lineArr, PrimitiveType.LineStrip);
                    window.Draw(person);
                    ShowStatistic(elapsed, person, font, window);
                    window.Draw(world);
                    window.Draw(mapCoordinates);
                    //window.Draw(mousePosition);

                    window.Display();
                }
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void InputEvents(Person person, MapCoordinates mapCoordinates)
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.W))
            {
                var personPosition = person.Position;
                personPosition.X += (float)Math.Sin(person.Direction) * person.Speed;
                personPosition.Y += (float)Math.Cos(person.Direction) * person.Speed;
                person.Position = personPosition;
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.S))
            {
                var personPosition = person.Position;
                personPosition.X -= (float)Math.Sin(person.Direction) * person.Speed;
                personPosition.Y -= (float)Math.Cos(person.Direction) * person.Speed;
                person.Position = personPosition;
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
            {
                var personPosition = person.Position;
                personPosition.Y -= person.SpeedStrafe;
                person.Position = personPosition;
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.Down))
            {
                var personPosition = person.Position;
                personPosition.Y += person.SpeedStrafe;
                person.Position = personPosition;
            }


            if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
            {
                var personPosition = person.Position;
                personPosition.X -= person.SpeedStrafe;
                person.Position = personPosition;
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
            {
                var personPosition = person.Position;
                personPosition.X += person.SpeedStrafe;
                person.Position = personPosition;
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.A))
            {
                person.Direction += person.SpeedTurn;
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.D))
            {
                person.Direction -= person.SpeedTurn;
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.M))
            {
                mapCoordinates.Visible = !mapCoordinates.Visible;
            }
        }

        private static void ShowStatistic(double elapsed, Person person, Font font, RenderWindow window)
        {
            var fps = 1d / elapsed;
            //Console.WriteLine(fps);
            var position = Mouse.GetPosition(window);
            var text = new Text($"Position: X ({person.Position.X:000.00}) Y ({person.Position.Y:000.00})," +
                                $" angle: {person.DirectionDegree:000.00}, FPS: {fps:00.00}, Mouse: ({position.X},{position.Y})", font, 12);
            text.Position = new Vector2f(300, 0);
            text.FillColor = Color.White;
            //text.OutlineThickness = 0.1f;
            window.Draw(text);
        }
    }

    //public class MousePosition : Drawable
    //{
    //    private readonly WindowBase _window;
    //    public Font Font = new Font(@"d:\Projects\Experimentals\Pseudo3dEngine\arial.ttf");

    //    public MousePosition(WindowBase window)
    //    {
    //        _window = window;
    //    }
    //    public void Draw(RenderTarget target, RenderStates states)
    //    {
    //        var position = Mouse.GetPosition(_window);
    //        var text = new Text($"({position.X},{position.Y})", Font, 6);
    //        text.Position = circleShape.Position;
    //        text.FillColor = Color.White;
    //        target.Draw(text);
    //    }
    //}
}
