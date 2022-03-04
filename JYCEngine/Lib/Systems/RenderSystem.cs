using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JYCEngine.StdLib
{
    public struct RenderSystem : ISystem
    {
        private Filter _filter;
        private Filter _cameraFilter;

        public void Init(EcsWorld world)
        {
            _cameraFilter = world.CreateFilter().Require<CameraComponent>().Require<PositionComponent>();
            _filter = world.CreateFilter().Require<ImageComponent>().Require<PositionComponent>();
        }

        public void Execute()
        {
            foreach (var camera in _cameraFilter.Matches())
            {
                var cameraPosition = camera.Get<PositionComponent>().position;
                var cameraBuffer = camera.Get<CameraComponent>().renderBuffer;
                var halfBufferSize = new Vector2(cameraBuffer.Width, cameraBuffer.Height) * 0.5f;
                foreach (var entity in _filter.Matches())
                {
                    var position = entity.Get<PositionComponent>().position;
                    var image = entity.Get<ImageComponent>();
                    position -= cameraPosition - halfBufferSize;
                    position -= image.pivot;
                    cameraBuffer.SetCharacters((int)position.x, (int)position.y, image.chars, false);
                }
                cameraBuffer.Blit();
                cameraBuffer.ClearBuffer();
            }
        }
    }
}
