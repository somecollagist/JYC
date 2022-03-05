namespace JYCEngine.StdLib;

[RequireComponents(typeof(PositionComponent))]
public struct ImageComponent
{
    public Vector2 pivot;
    public char[,] chars;
}