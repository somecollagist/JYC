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

        public void Init(EcsWorld world)
        {
            _filter = world.CreateFilter().Require<ImageComponent>().Require<PositionComponent>();
        }

        public void Execute()
        {
            foreach (var entity in _filter.Matches())
            {
                var position = entity.Get<PositionComponent>();
                var image = entity.Get<ImageComponent>();
                Engine.RenderBuffer.SetCharacters(position.x, position.y, image.chars, false);
            }
            Engine.RenderBuffer.Blit();
            Engine.RenderBuffer.ClearBuffer();
        }
    }
}
