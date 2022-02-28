using JYCEngine;
using JYCEngine.StdLib;

Engine.TargetFramerate = 35;

EcsWorld world = new EcsWorld();
Systems systems = new Systems(world)
    .Add(new AnimationSystem())
    .Add(new RenderSystem());
systems.Init();

Engine.Tick += systems.Execute;

// Create stuff
Random rand = new Random();
for (int i = 0; i < 10; i++)
{
    var entity = world.CreateEntity()
        .Replace(new PositionComponent() { x = Console.WindowWidth / 2 - 20 + i * 4, y = Console.WindowHeight / 2 - 10 + i * 2 })
        .Replace(new ImageComponent())
        .Replace(new AnimationComponent());
    ref AnimationComponent animation = ref entity.Get<AnimationComponent>();
    animation.frames = new List<AnimationFrame>();
    int frame = 0;
    foreach (char c in " .-~=&#@#&=~-.")
    {
        char[,] chars = new char[3, 3];
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                chars[x, y] = c;
            }
        }
        animation.frames.Add(new AnimationFrame() { chars = chars, frame = frame });
        //frame += rand.Next(3, 10);
        frame++;
    }
    animation.frameCount = frame;
    animation.framerate = 20;
}

Engine.Init();
Engine.Run();