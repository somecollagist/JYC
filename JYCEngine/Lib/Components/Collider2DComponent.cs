namespace JYCEngine.StdLib;

public struct Collider2DComponent
{
    public List<Vector2> points;
    public bool closed;
    public float boundingRadius;

    public Collider2DComponent()
    {
        points = new List<Vector2>();
        closed = true;
        boundingRadius = 0;
    }
}

public static class Collider2DComponentExtensions
{
    public static void RecalculateBounds(this ref Collider2DComponent collider)
    {
        collider.boundingRadius = 0;
        foreach (var point in collider.points)
        {
            collider.boundingRadius = Math.Max(collider.boundingRadius, point.SqrMagnitude());
        }
        collider.boundingRadius = (float)Math.Sqrt(collider.boundingRadius); // Only sqrt once for performance
    }
}