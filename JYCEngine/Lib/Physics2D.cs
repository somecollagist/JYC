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

        if (0 <= l && l <= 1 && 0 <= m && m <= 1) return Option<IntersectionPoint>.None;

        return Option<IntersectionPoint>.Some(new()
        {
            point = a + m * b,
            normal = b.Normal().Normalize()
        });
    }

    public struct IntersectionPoint
    {
        public Vector2 point;
        public Vector2 normal;
    }
}
