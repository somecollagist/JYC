using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JYCEngine.StdLib;

public struct Vector2
{
    public float x;
    public float y;

    public Vector2(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    public static Vector2 operator +(Vector2 a) => a;
    public static Vector2 operator -(Vector2 a) => new Vector2(-a.x, -a.y);

    public static Vector2 operator *(Vector2 a, float b) => new Vector2(a.x * b, a.y * b);
    public static Vector2 operator /(Vector2 a, float b) => new Vector2(a.x / b, a.y / b);
    public static Vector2 operator *(float a, Vector2 b) => b * a;

    public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.x + b.x, a.y + b.y);
    public static Vector2 operator -(Vector2 a, Vector2 b) => new Vector2(a.x - b.x, a.y - b.y);
    public static Vector2 operator *(Vector2 a, Vector2 b) => new Vector2(a.x * b.x, a.y * b.y);
    public static Vector2 operator /(Vector2 a, Vector2 b) => new Vector2(a.x / b.x, a.y / b.y);

    public static bool operator ==(Vector2 a, Vector2 b) => a.x == b.x && a.y == b.y;
    public static bool operator !=(Vector2 a, Vector2 b) => a.x != b.x || a.y != b.y;

    public static Vector2 Zero => new Vector2(0, 0);
    public static Vector2 One => new Vector2(1, 1);

    public static Vector2 Up => new Vector2(0, 1);
    public static Vector2 Down => new Vector2(0, -1);
    public static Vector2 Left => new Vector2(-1, 0);
    public static Vector2 Right => new Vector2(1, 0);

    public override string ToString()
    {
        return $"({x}, {y})";
    }
}

public static class Vector2Extensions
{
    const float DEG2RAD = (float)(Math.PI / 180f);

    public static float Magnitude(this in Vector2 v)
    {
        var sqrMagnitude = v.SqrMagnitude();
        return sqrMagnitude == 0 ? 0 : (float)Math.Sqrt(v.SqrMagnitude());
    }

    public static float SqrMagnitude(this in Vector2 v) => v.x * v.x + v.y * v.y;

    public static Vector2 Normalize(this in Vector2 v) => v / Magnitude(v);

    public static float Dot(this in Vector2 a, in Vector2 b) => a.x * b.x + a.y * b.y;

    public static float Cross(this in Vector2 a, Vector2 b) => a.x * b.y - a.y * b.x;

    public static Vector2 Rotate(this in Vector2 a, float angle) => new Vector2(a.x * (float)Math.Cos(angle*DEG2RAD) - a.y * (float)Math.Sin(angle*DEG2RAD), a.x * (float)Math.Sin(angle * DEG2RAD) + a.y * (float)Math.Cos(angle * DEG2RAD));

    public static Vector2 RotateAround(this in Vector2 vector, in Vector2 pivot, float angle) => (vector - pivot).Rotate(angle) + pivot;

    public static Vector2 Normal(this in Vector2 tangent) => new Vector2(-tangent.y, tangent.x);

    public static Vector2 Reflect(this in Vector2 vector, in Vector2 axis) => vector - 2 * vector.Dot(axis) * axis;

    public static Vector2 Min(this in Vector2 a, in Vector2 b) => new Vector2(Math.Min(a.x, b.x), Math.Min(a.y, b.y));

    public static Vector2 Max(this in Vector2 a, in Vector2 b) => new Vector2(Math.Max(a.x, b.x), Math.Max(a.y, b.y));
}