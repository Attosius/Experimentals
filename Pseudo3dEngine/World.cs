using SFML.Graphics;
using SFML.System;

namespace Pseudo3dEngine;

public class World : Drawable
{
    public List<Object2d> Objects = new List<Object2d>();

    public World()
    {
        var square = new Object2d();
        square.Points.Add(new Vector2f(25, 150));
        square.Points.Add(new Vector2f(100, 150));
        square.Points.Add(new Vector2f(100, 200));
        square.Points.Add(new Vector2f(25, 200));

        var square2 = new Object2d();
        square2.Points.Add(new Vector2f(150, 150));
        square2.Points.Add(new Vector2f(250, 150));
        square2.Points.Add(new Vector2f(250, 175));
        square2.Points.Add(new Vector2f(150, 175));
        Objects.Add(square);
        Objects.Add(square2);
    }

    public void Draw(RenderTarget target, RenderStates states)
    {
        foreach (var object2d in Objects)
        {

            target.Draw(object2d);
        }
    }
}