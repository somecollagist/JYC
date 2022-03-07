namespace JYCEngine.StdLib;

public struct AnimationSystem : ISystem
{
    private Filter _filter;

    public void Init(EcsWorld world)
    {
        _filter = world.CreateFilter()
            .Require<ImageComponent>()
            .Require<AnimationComponent>();
    }

    public void Execute()
    {
        foreach (var entity in _filter.Matches())
        {
            ref var animation = ref entity.Get<AnimationComponent>();
            ref var image = ref entity.Get<ImageComponent>();

            animation.currentFrame += Engine.DeltaTime * animation.framerate; // Update frame counter
            int frame = (int)(animation.currentFrame) % animation.frameCount;
            int index = animation.frames.Count - 1; // Find index of frame to be displayed
            for (int i = 1; i < animation.frames.Count; i++)
            {
                if (frame < animation.frames[i].frame)
                {
                    index = i - 1;
                    break;
                }
            }

            image.chars = animation.frames[index].chars; // Update image to show current frame
        }
    }
}

