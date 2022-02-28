using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JYCEngine.StdLib;

namespace JYCEngine.StdLib;

public struct RenderingSystem : ISystem
{
    public void Execute(double deltaTime)
    {
        Engine.RenderBuffer.ClearBuffer();

        var cameraEntity = Engine.Entities.With<CameraComponent>().With<TransformComponent>().First();
        var cameraTransform = cameraEntity.GetFirstComponent<TransformComponent>();

        Engine.Entities.With<ImageComponent>().With<TransformComponent>().ForEach(entity =>
        {
            var transform = entity.GetFirstComponent<TransformComponent>();
            var image = entity.GetFirstComponent<ImageComponent>();

            var rx = cameraTransform.X - transform.X;
            var ry = cameraTransform.Y - transform.Y;
            var rz = cameraTransform.Z - transform.Z; // 3rd Dimension currently unused

            Engine.RenderBuffer.SetCharacters((int)rx, (int)ry, image.Image);
        });

        Engine.RenderBuffer.Blit();
    }
}
