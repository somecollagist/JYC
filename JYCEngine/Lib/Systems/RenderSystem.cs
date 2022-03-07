using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JYCEngine.StdLib;

public struct RenderSystem : ISystem
{
    private Filter _filter;
    private Filter _cameraFilter;

    public void Init(EcsWorld world)
    {
        _cameraFilter = world.CreateFilter().Require<CameraComponent>().Require<PositionComponent>();
        _filter = world.CreateFilter().Require<ImageComponent>().Require<PositionComponent>();
    }

    // TODO: Add rotation
    // TODO: Add scaling/resampling of images
    // TODO: Add ordered/layered drawing (back to front)
    // TODO: Add culling for offscreen images
    // TODO: Possibly add a depth buffer to help minimize overdraw (front to back rendering). Could maybe go in RenderBuffer class
    public void Execute()
    {
        foreach (var camera in _cameraFilter.Matches()) // Multiple camera support (multiplayer or splitscreen use? maybe for minimap effects too)
        {
            var cameraPosition = camera.Get<PositionComponent>().position;
            var cameraBuffer = camera.Get<CameraComponent>().renderBuffer;
            var halfBufferSize = new Vector2(cameraBuffer.Width, cameraBuffer.Height) * 0.5f;
            foreach (var entity in _filter.Matches())
            {
                // Calculate position on screen
                var position = entity.Get<PositionComponent>().position;
                var image = entity.Get<ImageComponent>();
                position -= cameraPosition - halfBufferSize;
                position -= image.pivot;
                cameraBuffer.SetCharacters((int)position.x, (int)position.y, image.chars, false); // Draw entity
            }
            cameraBuffer.Blit(); // Send pixels to screen
            cameraBuffer.ClearBuffer(); // Clear internal buffer for next frame
        }
    }
}

