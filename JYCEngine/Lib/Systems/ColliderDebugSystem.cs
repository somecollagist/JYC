using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JYCEngine.StdLib;

public struct ColliderDebugSystem : ISystem
{
    private Filter _filter;

    public void Init(EcsWorld world)
    {
        _filter = world.CreateFilter()
            .Require<PositionComponent>()
            .Require<Collider2DComponent>()
            .Require<ImageComponent>();
    }

    public void Execute()
    {
        foreach (var entity in _filter.Matches())
        {
            var position = entity.Get<PositionComponent>().position;
            ref var collider = ref entity.Get<Collider2DComponent>();
            ref var image = ref entity.Get<ImageComponent>();

            List<(int x, int y, char c)> imagePoints = new List<(int x, int y, char c)>();


            for (int i = 0; i < collider.points.Count - 1; i++)
                DrawLine(imagePoints, collider.points[i], collider.points[i + 1]);
            if (collider.closed) DrawLine(imagePoints, collider.points[^1], collider.points[0]);

            int minX = imagePoints[0].x;
            int minY = imagePoints[0].y;
            int maxX = imagePoints[0].x;
            int maxY = imagePoints[0].y;
            foreach (var point in imagePoints)
            {
                if (point.x < minX) minX = point.x;
                if (point.y < minY) minY = point.y;
                if (point.x > maxX) maxX = point.x;
                if (point.y > maxY) maxY = point.y;
            }
            int sizeX = maxX - minX + 1;
            int sizeY = maxY - minY + 1;

            image.chars = new char[sizeX, sizeY];

            foreach (var point in imagePoints)
            {
                image.chars[point.x - minX, point.y - minY] = point.c;
            }
            image.pivot = new Vector2(-minX, -minY);
        }
    }

    void DrawLine(List<(int x, int y, char c)> image, Vector2 from, Vector2 to)
    {
        float xdiff = Math.Abs(from.x - to.x);
        float ydiff = Math.Abs(from.y - to.y);
        if (ydiff > xdiff)
        {
            Vector2 lower = from.y < to.y ? from : to;
            Vector2 upper = from.y < to.y ? to : from;
            float dx = (upper.x - lower.x) / (upper.y - lower.y);
            float x = lower.x;
            for (float y = lower.y; y <= upper.y; y++)
            {
                image.Add(((int)x, (int)y, '+'));
                x += dx;
            }
            image.Add(((int)lower.x, (int)lower.y, '@'));
            image.Add(((int)upper.x, (int)upper.y, '@'));
        }
        else
        {
            Vector2 left = from.x < to.x ? from : to;
            Vector2 right = from.x < to.x ? to : from;
            float dy = (right.y - left.y) / (right.x - left.x);
            float y = left.y;
            for (float x = left.x; x <= right.x; x++)
            {
                image.Add(((int)x, (int)y, '+'));
                y += dy;
            }
            image.Add(((int)left.x, (int)left.y, '@'));
            image.Add(((int)right.x, (int)right.y, '@'));
        }
    }
}

