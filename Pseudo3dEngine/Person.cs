using SFML.Graphics;
using SFML.System;

namespace Pseudo3dEngine;

public class Person : Drawable
{
    public Vector2f Position { get; set; } = new Vector2f(100, 0);
    public double Fov { get; set; } = 3.14 / 3;
    public float Direction { get; set; } = 0;
    public float Speed = 6f;
    public float SpeedTurn = 0.1f;

    public float SpeedStrafe = 5f;
    public float DistanceView = 200f;

    public float DirectionDegree => Direction * 180 / (float)Math.PI;

    public void Draw(RenderTarget target, RenderStates states)
    {
        var radius = DrawPerson(target);

        var center = new Vector2f(Position.X + radius, Position.Y + radius);

        var viewSector = new Object2d();
        viewSector.Points.Add(center);

        var leftViewAngle = Direction - Fov / 2;
        var leftPoint = GetPointAtAngleAndDistance(center, leftViewAngle, DistanceView);
        viewSector.Points.Add(leftPoint);

        var delta = Fov / 10;
        var currentAngle = leftViewAngle;
        for (int i = 0; i < 10; i++)
        {
            var point = GetPointAtAngleAndDistance(center, currentAngle, DistanceView);
            currentAngle += delta;
            viewSector.Points.Add(point);
        }

        var rightViewAngle = Direction + Fov / 2;
        var rightPoint = GetPointAtAngleAndDistance(center, rightViewAngle, DistanceView);
        viewSector.Points.Add(rightPoint);
        target.Draw(viewSector);

        DrawDirection(target, states, center);
    }

    private static Vector2f GetPointAtAngleAndDistance(Vector2f center, double angle, float distance)
    {
        var x = center.X + (float)Math.Sin(angle) * distance;
        var y = center.Y + (float)Math.Cos(angle) * distance;
        return new Vector2f(x, y);
    }

    private void DrawDirection(RenderTarget target, RenderStates states, Vector2f center)
    {
        var xDir = center.X + (float)Math.Sin(Direction) * 150f;
        var yDir = center.Y + (float)Math.Cos(Direction) * 150f;
        var lineArr = new Vertex[]
        {
            new(center),
            new(new Vector2f(xDir, yDir)),
        };
        target.Draw(lineArr, PrimitiveType.LineStrip, states);
    }

    private float DrawPerson(RenderTarget target)
    {
        var radius = 10f;
        var person = new CircleShape(radius);
        person.FillColor = Color.Red;
        person.Position = Position;
        target.Draw(person);
        return radius;
    }
}