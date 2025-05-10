using System.Diagnostics;
using SFML.Graphics;
using SFML.System;

namespace Pseudo3dEngine;

public class MapCoordinates : Drawable
{
    public List<CircleShape> CircleShapes = new();
    public int CellSize = 25;
    public Font Font;
    private bool _visible;
    private readonly Stopwatch _visibleChangeTime = Stopwatch.StartNew();
    public bool Visible
    {
        get => _visible;
        set
        {
            if (_visibleChangeTime.ElapsedMilliseconds < 250)
            {
                return;
            }
            _visibleChangeTime.Restart();
            _visible = value;
        }
    }

    public MapCoordinates(uint screenHeight, uint screenWidth)
    {
        //Font = new Font(@"d:\Projects\Experimentals\Pseudo3dEngine\arial.ttf");
        Font = new Font(@"d:\Projects\Experimentals\Pseudo3dEngine\cour.ttf");
        for (int i = 0; i < screenHeight; i+=CellSize)
        {
            for (int j = 0; j < screenWidth; j += CellSize)
            {
                var circleShape = new CircleShape(1);
                circleShape.FillColor = new Color(128, 128, 128);
                circleShape.Position = new Vector2f(i, j);
                CircleShapes.Add(circleShape);
            }
        }
    }


    public void Draw(RenderTarget target, RenderStates states)
    {
        if (!Visible)
        {
            return;
        }

        foreach (var circleShape in CircleShapes)
        {
            if (circleShape.Position.X % 50 == 0 && circleShape.Position.Y % 50 == 0)
            {
                circleShape.FillColor = Color.White;
                if (circleShape.Position.X != 0 && circleShape.Position.Y != 0)
                {
                    var text = new Text($"({circleShape.Position.X},{circleShape.Position.Y})", Font, 8);
                    text.Position = new Vector2f(circleShape.Position.X - CellSize / 2f, circleShape.Position.Y + 2);
                    text.FillColor = Color.White;
                    target.Draw(text);
                        
                }
            }
            target.Draw(circleShape);
        }
    }
}