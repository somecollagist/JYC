using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JYCEngine.StdLib;

public static class Physics2D
{
    public static Option<IntersectionPoint> LineLineIntersection(Vector2 aFrom, Vector2 aTo, Vector2 bFrom, Vector2 bTo)
    {
        var a = aFrom;
        var b = aTo - aFrom;
        var c = bFrom;
        var d = bTo - bFrom;

        var m = (c - a).Cross(b) / b.Cross(d);
        var l = (a - c).Cross(d) / d.Cross(b);

        if (0 > l || l > 1 || 0 > m || m > 1) return Option<IntersectionPoint>.None;

        return Option<IntersectionPoint>.Some(new()
        {
            point = a + m * b,
            normal = -d.Normal().Normalize()
        });
    }

    public static Option<IntersectionPoint[]> LineCircleIntersection(Vector2 from, Vector2 to, Vector2 centre, float radius)
    {
        var a = from;
        var b = from - to;
        var d = from - centre;

        var A = b.Dot(b);
        var B = 2 * b.Dot(d);
        var C = d.Dot(d) - radius * radius;

        var det = B * B - 4 * A * C;
        if (det < 0 || A == 0) return Option<IntersectionPoint[]>.None;
        if (det == 0)
        {
            var t = -B / (2 * A);
            var point = a + b * t;
            if (0 > t || 1 < t) return Option<IntersectionPoint[]>.None;
            return Option<IntersectionPoint[]>.Some(new IntersectionPoint[] {
                new()
                {
                    point = point,
                    normal = (point - centre).Normalize()
                }
            });
        }
        // det > 0
        {
            var t1 = (float)(-B + Math.Sqrt(det)) / (2 * A);
            var point1 = a + b * t1;
            List<IntersectionPoint> points = new();
            if (0 <= t1 && t1 <= 1) points.Add(new() { point = point1, normal = (point1 - centre).Normalize() });

            var t2 = (float)(-B - Math.Sqrt(det)) / (2 * A);
            var point2 = a + b * t2;
            if (0 <= t2 && t2 <= 1) points.Add(new() { point = point2, normal = (point2 - centre).Normalize() });

            if (points.Count == 0) return Option<IntersectionPoint[]>.None;
            return Option<IntersectionPoint[]>.Some(points.ToArray());
        }
    }

    public struct IntersectionPoint
    {
        public Vector2 point;
        public Vector2 normal;
    }
}
