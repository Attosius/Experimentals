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
                rectangle.Size = new Vector2f(200f, 500f);
                rectangle.Position = new Vector2f(30, 60);
                rectangle.OutlineThickness = 2;
                rectangle.OutlineColor = new Color(255, 255, 255);


                var person = new CircleShape(10);
                person.FillColor = Color.Red;
                var personPosition = new Vector2f(10, 10);
                person.Position = personPosition;

                var line = new RectangleShape (new Vector2f(150, 5));
                line.Rotation = 45;


                var lineArr = new Vertex[]
                {
                    new Vertex(new Vector2f(10, 50)),
                    new Vertex(new Vector2f(150, 150)),
                    new Vertex(new Vector2f(50, 10))
                };
                
                // Start the game loop
                while (window.IsOpen)
                {
                    // Process events
                    window.DispatchEvents();
                    window.Closed += (object? sender, EventArgs e) => window.Close();


                    if (Keyboard.IsKeyPressed(Keyboard.Key.W) || Keyboard.IsKeyPressed(Keyboard.Key.Up))
                    {
                        // left key is pressed: move our character
                        personPosition.Y -= 0.1f;
                        person.Position = personPosition;
                    }
                    if (Keyboard.IsKeyPressed(Keyboard.Key.S) || Keyboard.IsKeyPressed(Keyboard.Key.Down))
                    {
                        // left key is pressed: move our character
                        personPosition.Y += 0.1f;
                        person.Position = personPosition;
                    }


                    if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
                    {
                        // left key is pressed: move our character
                        personPosition.X -= 0.1f;
                        person.Position = personPosition;
                    }
                    if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
                    {
                        // left key is pressed: move our character
                        personPosition.X += 0.1f;
                        person.Position = personPosition;
                    }

                    // Clear screen
                    window.Clear();
 
                    // Draw the sprite
                    //window.Draw(sprite);
 
                    // Draw the string
                    window.Draw(text);
                    //window.Draw(circleShape);
                    window.Draw(rectangle);
                    window.Draw(person);
                    //window.Draw(line);
                    window.Draw(lineArr, PrimitiveType.LineStrip);
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
}
