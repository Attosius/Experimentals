using SFML.Graphics;
using SFML.System;

namespace Pseudo3dEngine;

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