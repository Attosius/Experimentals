using SFML.Graphics;
using SFML.System;

namespace Pseudo3dEngine;

public class Object2d : Drawable
{
    public List<Vector2f> Points = new ();
    public Color FillColor = new Color(255, 175, 174);
    public Color OutlineColor = new Color(255, 255, 255);

    public void Draw(RenderTarget target, RenderStates states)
    {
        var convexShape = new ConvexShape();
        convexShape.SetPointCount((uint)Points.Count);
        for (var i = 0; i < Points.Count; i++)
        {
            var vector2F = Points[i];
            convexShape.SetPoint((uint)i, vector2F);
        }
        convexShape.FillColor = FillColor;
        convexShape.OutlineThickness = 2;
        convexShape.OutlineColor = OutlineColor;
        target.Draw(convexShape);
    }
}