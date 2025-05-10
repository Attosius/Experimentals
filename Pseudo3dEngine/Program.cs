using System;
using System.Diagnostics;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Pseudo3dEngine
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Hello, World!");
                var videoMode = new VideoMode(800, 600);
                var window = new RenderWindow(videoMode, "Sfml window");
                window.Display();

                Texture texture = new Texture("d:\\Projects\\Experimentals\\Pseudo3dEngine\\cold_heart.jpeg");
                Sprite sprite = new Sprite(texture);

                var font = new Font("d:\\Projects\\Experimentals\\Pseudo3dEngine\\cour.ttf");

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
                // Start the game loop
                while (window.IsOpen)
                {
                    var elapsed = sw.ElapsedMilliseconds / (double)1000;
                    sw.Restart();
                    Thread.Sleep(10);

                    window.DispatchEvents();
                    window.Closed += (sender, _) => ((RenderWindow)sender!).Close();
                    
                    InputEvents(person);

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

                    window.Display();
                }
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void InputEvents(Person person)
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
        }

        private static void ShowStatistic(double elapsed, Person person, Font font, RenderWindow window)
        {
            var fps = 1d / elapsed;
            //Console.WriteLine(fps);
            var text = new Text($"Position: X ({person.Position.X:000.00}) Y ({person.Position.Y:000.00})," +
                                $" angle: {person.DirectionDegree:000.00}, FPS: {fps:00.00}", font, 12);
            text.Position = new Vector2f(0, 0);
            text.FillColor = Color.White;
            //text.OutlineThickness = 0.1f;
            window.Draw(text);
        }
    }

    public class Object2d : Drawable
    {
        public List<Vector2f> Points = new ();

        public void Draw(RenderTarget target, RenderStates states)
        {
            var convexShape = new ConvexShape();
            convexShape.SetPointCount((uint)Points.Count);
            for (var i = 0; i < Points.Count; i++)
            {
                var vector2F = Points[i];
                convexShape.SetPoint((uint)i, vector2F);
            }
            convexShape.FillColor = new Color(255, 175, 174);
            convexShape.OutlineThickness = 2;
            convexShape.OutlineColor = new Color(255, 255, 255);
            target.Draw(convexShape);
        }
    }

    public class World : Drawable
    {
        public List<Object2d> Objects = new List<Object2d>();

        public World()
        {
            var square = new Object2d();
            square.Points.Add(new Vector2f(50, 150));
            square.Points.Add(new Vector2f(100, 150));
            square.Points.Add(new Vector2f(100, 200));
            square.Points.Add(new Vector2f(50, 200));
            Objects.Add(square);
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            foreach (var object2d in Objects)
            {

                target.Draw(object2d);
            }
        }
    }

    public class CameraMan : Transformable, Drawable
    {
        public void Draw(RenderTarget target, RenderStates states)
        {
            var lineArr = new Vertex[]
            {
                new Vertex(new Vector2f(10, 50)),
                new Vertex(new Vector2f(150, 150)),
                //new Vertex(new Vector2f(50, 10)),
                //new Vertex(new Vector2f(520, 130)),
                //new Vertex(new Vector2f(530, 160))
            };
            target.Draw(lineArr, PrimitiveType.LineStrip, states);
        }
    }
}
