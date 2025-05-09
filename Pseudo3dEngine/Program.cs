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
                var text = new Text("Hello SFML", font, 50);
                var circleShape = new CircleShape(50);
                circleShape.FillColor = Color.Green;


                var rectangle = new RectangleShape(new Vector2f(120f, 50f));
                rectangle.FillColor = new Color(255, 175, 174);
                rectangle.Size = new Vector2f(10, 10);
                rectangle.Position = new Vector2f(0, 0);
                rectangle.OutlineThickness = 2;
                rectangle.OutlineColor = new Color(255, 255, 255);



                var line = new RectangleShape (new Vector2f(10, 133));
                line.Rotation = 45;


                var lineArr = new Vertex[]
                {
                    new Vertex(new Vector2f(10, 50)),
                    new Vertex(new Vector2f(150, 150)),
                    new Vertex(new Vector2f(50, 10))
                };

                var person = new Person();

                var cam = new CameraMan();
                
                // Start the game loop
                while (window.IsOpen)
                {
                    // Process events
                    window.DispatchEvents();
                    window.Closed += (object? sender, EventArgs e) => ((RenderWindow)sender!).Close();


                    if (Keyboard.IsKeyPressed(Keyboard.Key.W) )
                    {
                        var personPosition = person.Position;
                        personPosition.X += (float)Math.Sin(person.Direction) * 0.01f;
                        personPosition.Y += (float)Math.Cos(person.Direction) * 0.01f;
                        person.Position = personPosition;
                    }
                    if (Keyboard.IsKeyPressed(Keyboard.Key.S))
                    {
                        var personPosition = person.Position;
                        personPosition.X -= (float)Math.Sin(person.Direction) * 0.01f;
                        personPosition.Y -= (float)Math.Cos(person.Direction) * 0.01f;
                        person.Position = personPosition;
                    }

                    if ( Keyboard.IsKeyPressed(Keyboard.Key.Up))
                    {
                        var personPosition = person.Position;
                        personPosition.Y -= 0.1f;
                        person.Position = personPosition;
                    }
                    if (Keyboard.IsKeyPressed(Keyboard.Key.Down))
                    {
                        var personPosition = person.Position;
                        personPosition.Y += 0.1f;
                        person.Position = personPosition;
                    }


                    if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
                    {
                        var personPosition = person.Position;
                        personPosition.X -= 0.1f;
                        person.Position = personPosition;
                    }
                    if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
                    {
                        var personPosition = person.Position;
                        personPosition.X += 0.1f;
                        person.Position = personPosition;
                    }

                    if (Keyboard.IsKeyPressed(Keyboard.Key.A))
                    {
                        person.Direction += 0.001f;
                    }
                    if (Keyboard.IsKeyPressed(Keyboard.Key.D))
                    {
                        person.Direction -= 0.001f;
                    }
                    // Clear screen
                    window.Clear();
 
                    // Draw the sprite
                    //window.Draw(sprite);
 
                    // Draw the string
                    //window.Draw(text);
                    window.Draw(circleShape);
                    window.Draw(rectangle);
                    window.Draw(person);
                    //window.Draw(line);
                    window.Draw(cam);
                    //window.Draw(lineArr, PrimitiveType.LineStrip);
                    // Update the window
                    window.Display();
                }
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        
    }
    public class Person : Drawable
    {
        public Vector2f Position { get; set; }
        public double Fov { get; set; } = 3.14 / 3;
        public float Direction { get; set; } = 0;

        public float DirectionDegree => Direction * 180 / (float)Math.PI;

        public void Draw(RenderTarget target, RenderStates states)
        {
            var radius = 10f;
            var person = new CircleShape(radius);
            person.FillColor = Color.Red;
            person.Position = Position;
            target.Draw(person);
            //Console.WriteLine($"Position: {person.Position}, angle: {DirectionDegree}");
            var center = new Vector2f(Position.X + radius, Position.Y + radius);
            //Console.WriteLine(Position);
            var xDir = center.X + (float)Math.Sin(Direction) * 50f;
            var yDir = center.Y + (float)Math.Cos(Direction) * 50f;
            var lineArr = new Vertex[]
            {
                new Vertex(center),
                new Vertex(new Vector2f(xDir, yDir)),
            };
            target.Draw(lineArr, PrimitiveType.LineStrip, states);
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
