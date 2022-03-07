namespace JYCEngine.StdLib;

/// <summary>
/// Class for handling 2D vectors
/// </summary>
public struct Vector2
{
    public float x;
    public float y;

    public Vector2(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    // Operator overrides
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

    // Constant getters for unit vectors
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

/// <summary>
/// Helper functions for Vector2s
/// </summary>
public static class Vector2Extensions
{
    const float DEG2RAD = (float)(Math.PI / 180f); // Convert degrees to radians

    /// <summary>
    /// Get the magnitude of a vector
    /// </summary>
    /// <param name="v">The vector</param>
    /// <returns></returns>
    public static float Magnitude(this in Vector2 v)
    {
        var sqrMagnitude = v.SqrMagnitude();
        return sqrMagnitude == 0 ? 0 : (float)Math.Sqrt(v.SqrMagnitude());
    }

    /// <summary>
    /// Get the square magnitude of a vector
    /// </summary>
    /// <param name="v">The vector</param>
    /// <returns></returns>
    /// <remarks>
    /// Always try to use this over <see cref="Magnitude(in Vector2)"/>, as this doesn't use
    /// a square root, so is much faster
    /// </remarks>
    public static float SqrMagnitude(this in Vector2 v) => v.x * v.x + v.y * v.y;

    /// <summary>
    /// Unit vector in the direction of <paramref name="v"/>
    /// </summary>
    /// <param name="v">Direction vector</param>
    /// <returns></returns>
    public static Vector2 Normalize(this in Vector2 v) => v / Magnitude(v);

    /// <summary>
    /// Vector dot product of <paramref name="a"/> and <paramref name="b"/>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float Dot(this in Vector2 a, in Vector2 b) => a.x * b.x + a.y * b.y;

    /// <summary>
    /// Vector cross product of <paramref name="a"/> and <paramref name="b"/>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float Cross(this in Vector2 a, Vector2 b) => a.x * b.y - a.y * b.x;

    /// <summary>
    /// Rotate vector around (0, 0)
    /// </summary>
    /// <param name="a">The vector</param>
    /// <param name="angle">The angle to rotate by</param>
    /// <returns></returns>
    public static Vector2 Rotate(this in Vector2 a, float angle) => new Vector2(a.x * (float)Math.Cos(angle*DEG2RAD) - a.y * (float)Math.Sin(angle*DEG2RAD), a.x * (float)Math.Sin(angle * DEG2RAD) + a.y * (float)Math.Cos(angle * DEG2RAD));

    /// <summary>
    /// Rotate vector around <paramref name="pivot"/>
    /// </summary>
    /// <param name="a">The vector</param>
    /// <param name="pivot">The vector to rotate around</param>
    /// <param name="angle">The angle to rotate by</param>
    /// <returns></returns>
    public static Vector2 RotateAround(this in Vector2 vector, in Vector2 pivot, float angle) => (vector - pivot).Rotate(angle) + pivot;

    /// <summary>
    /// A normal to a vector
    /// </summary>
    /// <param name="tangent"></param>
    /// <returns></returns>
    /// <remarks>
    /// Note that this is only one of two normals. The second one is the inverse (negative) of this
    /// </remarks>
    public static Vector2 Normal(this in Vector2 tangent) => new Vector2(-tangent.y, tangent.x);

    /// <summary>
    /// Reflect a vector across an axis
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="axis"></param>
    /// <returns></returns>
    public static Vector2 Reflect(this in Vector2 vector, in Vector2 axis) => vector - 2 * vector.Dot(axis) * axis;

    public static Vector2 Min(this in Vector2 a, in Vector2 b) => new Vector2(Math.Min(a.x, b.x), Math.Min(a.y, b.y));

    public static Vector2 Max(this in Vector2 a, in Vector2 b) => new Vector2(Math.Max(a.x, b.x), Math.Max(a.y, b.y));
}