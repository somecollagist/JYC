namespace JYCEngine.StdLib;

public struct PhysicsSystem : ISystem
{
    private Filter _staticFilter;
    private Filter _kinematicFilter;
    private Filter _dynamicFilter;

    public void Init(EcsWorld world)
    {
        _staticFilter = world.CreateFilter()
            .Require<PositionComponent>()
            .Require<Collider2DComponent>();

        _kinematicFilter = world.CreateFilter()
            .Require<PositionComponent>()
            .Require<VelocityComponent>();

        _dynamicFilter = world.CreateFilter()
            .Require<PositionComponent>()
            .Require<VelocityComponent>()
            .Require<Collider2DComponent>();
    }

    public void Execute()
    {
        NarrowPhase();

        foreach (var entity in _kinematicFilter.Matches().Concat(_dynamicFilter.Matches()))
        {
            ref var position = ref entity.Get<PositionComponent>().position;
            var velocityComponent = entity.Get<VelocityComponent>();
            var velocity = velocityComponent.velocity;
            var angularVelocity = velocityComponent.angularVelocity;

            position += velocity * Engine.DeltaTime;

            if (entity.Has<Collider2DComponent>())
            {
                ref var collider = ref entity.Get<Collider2DComponent>();
                for (int i = 0; i < collider.points.Count; i++)
                {
                    collider.points[i] = collider.points[i].Rotate(angularVelocity * Engine.DeltaTime);
                }
            }
        }
    }

    private IEnumerable<(Entity a, Entity b)> BroadPhase()
    {
        // Process collisions only on dynamic bodies.
        // Dynamic bodies can collide with static bodies, but not vice versa
        foreach (var entity in _dynamicFilter.Matches())
        {
            var position = entity.Get<PositionComponent>().position;
            var velocity = entity.Get<VelocityComponent>().velocity;
            var collider = entity.Get<Collider2DComponent>();

            position += velocity * Engine.DeltaTime;

            foreach (var other in _staticFilter.Matches().Concat(_dynamicFilter.Matches()))
            {
                if (entity.ID == other.ID) break;

                var otherPosition = other.Get<PositionComponent>().position;
                var otherCollider = other.Get<Collider2DComponent>();

                if (other.Has<VelocityComponent>())
                {
                    var otherVelocity = other.Get<VelocityComponent>().velocity;
                    otherPosition += otherVelocity * Engine.DeltaTime;
                }

                if ((otherPosition - position).Magnitude() <= collider.boundingRadius + otherCollider.boundingRadius)
                {
                    // Possible collision
                    yield return (entity, other);
                }
            }
        }
    }

    private void NarrowPhase()
    {
        foreach ((Entity a, Entity b) in BroadPhase())
        {
            ref var aPosition = ref a.Get<PositionComponent>().position;
            ref var aVelocityComponent = ref a.Get<VelocityComponent>();
            ref var aVelocity = ref aVelocityComponent.velocity;
            ref var aAngularVelocity = ref aVelocityComponent.angularVelocity;
            var aCollider = a.Get<Collider2DComponent>();

            var bPosition = b.Get<PositionComponent>().position;
            var bCollider = b.Get<Collider2DComponent>();
            bool isDynamic = b.Has<VelocityComponent>();

            HitInfo hitInfo;

            int i = 0;
            do
            {
                if (isDynamic)
                {
                    var bVelocityComponent = b.Get<VelocityComponent>();
                    var bVelocity = bVelocityComponent.velocity;
                    var bAngularVelocity = bVelocityComponent.angularVelocity;

                    hitInfo = GetHitPoint(
                        aPosition, aVelocity, aAngularVelocity, aCollider,
                        bPosition, bVelocity, bAngularVelocity, bCollider);
                }
                else
                {
                    hitInfo = GetHitPoint(
                        aPosition, aVelocity, aAngularVelocity, aCollider,
                        bPosition, Vector2.Zero, 0, bCollider);
                }
                
                if (hitInfo.hit) //Collision occurred
                {
                    aPosition += hitInfo.normal * aVelocity.Magnitude() * Engine.DeltaTime;
                    aVelocity = aVelocity.Reflect(hitInfo.normal);
                    aAngularVelocity = 120 * hitInfo.normal.Dot(aVelocity.Normalize()) * - Math.Sign(aAngularVelocity);
                    if (isDynamic)
                    {
                        ref var bVelocityComponent = ref b.Get<VelocityComponent>();
                        ref var bVelocity = ref bVelocityComponent.velocity;
                        ref var bAngularVelocity = ref bVelocityComponent.angularVelocity;

                        bPosition += -hitInfo.normal * bVelocity.Magnitude() * Engine.DeltaTime;
                        bVelocity = bVelocity.Reflect(-hitInfo.normal);
                        bAngularVelocity = 120 * -hitInfo.normal.Dot(bVelocity.Normalize()) * -Math.Sign(bAngularVelocity);
                    }
                }
                i++;
            } while (hitInfo.hit && i < 50);
        }
    }

    Vector2 GetPointAfterStep(Vector2 p, Vector2 position, Vector2 velocity, float angularVelocity)
    {
        return position + (p + velocity * Engine.DeltaTime).Rotate(angularVelocity * Engine.DeltaTime);
    }

    HitInfo GetHitPoint(Vector2 aPosition, Vector2 aVelocity, float aAngularVelocity, Collider2DComponent aCollider, Vector2 bPosition, Vector2 bVelocity, float bAngularVelocity, Collider2DComponent bCollider)
    {
        bool hit = false;
        float angle = 0f;
        float nearestSqrDistance = float.MaxValue;
        Vector2 nearestPoint = Vector2.Zero;
        Vector2 normal = Vector2.Zero;
        for (int i = 0; i < aCollider.points.Count; i++)
        {
            Vector2 startA = GetPointAfterStep(aCollider.points[i], aPosition, aVelocity, aAngularVelocity);
            Vector2 endA = GetPointAfterStep(aCollider.points[(i + 1) % aCollider.points.Count], aPosition, aVelocity, aAngularVelocity);
            for (int j = 0; j < bCollider.points.Count; j++)
            {
                Vector2 startB = GetPointAfterStep(bCollider.points[j], bPosition, bVelocity, bAngularVelocity);
                Vector2 endB = GetPointAfterStep(bCollider.points[(j + 1) % bCollider.points.Count], bPosition, bVelocity, bAngularVelocity);
                (bool lineHit, Vector2 point) = LineTest(startA, endA, startB, endB);
                if (lineHit)
                {
                    float sqrDistance = (point - aPosition).SqrMagnitude();
                    if (sqrDistance < nearestSqrDistance)
                    {
                        nearestSqrDistance = sqrDistance;
                        nearestPoint = point;
                        hit = true;
                        normal = (startB - endB).Normal().Normalize();
                        if (normal.Dot(startB - endB) != ((startB + endB) * 0.5f - aPosition).Dot(startB - endB)) normal = -normal;
                    }
                }
            }
        }
        return new HitInfo()
        {
            hit = hit,
            point = nearestPoint,
            normal = normal
        };
    }

    struct HitInfo
    {
        public bool hit;
        public Vector2 point;
        public Vector2 normal;
    }

    (bool hit, Vector2 point) LineTest(Vector2 start1, Vector2 end1, Vector2 start2, Vector2 end2)
    {
        var a = start1;
        var b = end1 - start1;
        var c = start2;
        var d = end2 - start2;

        var m = (c - a).Cross(b) / b.Cross(d);
        var l = (a - c).Cross(d) / d.Cross(b);

        return (0 <= l && l <= 1 && 0 <= m && m <= 1, a + b * m);
    }

}