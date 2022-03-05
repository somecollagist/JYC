namespace JYCEngine.StdLib;

[RequireComponents(typeof(ImageComponent))]
public struct AnimationComponent
{
    public float framerate;
    public List<AnimationFrame> frames;
    public float currentFrame;
    public int frameCount;
}

public struct AnimationFrame
{
    public int frame;
    public char[,] chars;
}