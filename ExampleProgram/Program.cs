using JYCEngine;
using JYCEngine.StdLib;

// Settings
Engine.TargetFramerate = 60;

EcsWorld world = new EcsWorld(); // Create new 'world'
Systems systems = new Systems(world) // Register systems
    .Add(new AnimationSystem())
    .Add(new PhysicsSystem())
    .Add(new ColliderDebugSystem())
    .Add(new RenderSystem());
systems.Init();
Engine.Tick += systems.Execute; 

Engine.Init(); // Initialise engine

var camera = world.CreateEntity() // Create camera
    .Replace(new CameraComponent(Engine.RenderBuffer))
    .Replace(new PositionComponent() { position = Vector2.Zero })
    .Replace(new VelocityComponent());

// Add (very poorly coded) input bindings to move the camera
Input.RegisterBinding('w', () => camera.Get<VelocityComponent>().velocity += Vector2.Up * 1f);
Input.RegisterBinding('a', () => camera.Get<VelocityComponent>().velocity += Vector2.Left * 1f);
Input.RegisterBinding('s', () => camera.Get<VelocityComponent>().velocity += Vector2.Down * 1f);
Input.RegisterBinding('d', () => camera.Get<VelocityComponent>().velocity += Vector2.Right * 1f);

// Some shapes
var triangle = new List<Vector2>() { new Vector2(-1, 0), new Vector2(1, 1), new Vector2(1, -1) };
var square = new List<Vector2>() { new Vector2(-1, -1), new Vector2(-1, 1), new Vector2(1, 1), new Vector2(1, -1) };

// Random helper functionality
Random rand = new Random();
Vector2 RandomVector(float size) => new Vector2((rand.NextSingle() * 2 - 1) * size, (rand.NextSingle() * 2 - 1) * size);

// Create some shapes
CreateStaticShape(new Vector2(-120, 0), 65, square);
CreateStaticShape(new Vector2(120, 0), 65, square);
CreateStaticShape(new Vector2(0, 120), 65, square);
CreateStaticShape(new Vector2(0, -120), 65, square);
for (int i = 0; i < 5; i++)
{
    CreateShape(RandomVector(20), rand.Next(3, 8), RandomVector(5), (rand.NextSingle() * 2 - 1) * 120, square);
    CreateShape(RandomVector(20), rand.Next(3, 8), RandomVector(5), (rand.NextSingle() * 2 - 1) * 120, triangle);
}

// Helper functions for shapes
void CreateShape(Vector2 position, int scale, Vector2 velocity, float angularVelocity, List<Vector2> points)
{
    var shape = world.CreateEntity()
        .Replace(new PositionComponent() { position = position })
        .Replace(new VelocityComponent() { velocity = velocity, angularVelocity = angularVelocity })
        .Replace(new ImageComponent())
        .Replace(new Collider2DComponent() { points = points.Select(x => x * scale).ToList(), closed = true });
    shape.Get<Collider2DComponent>().RecalculateBounds();
}

void CreateStaticShape(Vector2 position, int scale, List<Vector2> points)
{
    var shape = world.CreateEntity()
        .Replace(new PositionComponent() { position = position })
        .Replace(new ImageComponent())
        .Replace(new Collider2DComponent() { points = points.Select(x => x * scale).ToList(), closed = true });
    shape.Get<Collider2DComponent>().RecalculateBounds();
}

// Run the engine
Engine.Run();