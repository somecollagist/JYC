namespace JYCEngine;

public struct Entity
{
    public int ID;
    public EcsWorld Owner;
    public Dictionary<Type, int> Components;    
}

public static class EntityExtensions
{
    public static ref T Get<T>(this Entity entity)
    {
        if (!entity.Components.ContainsKey(typeof(T)))
        {
            if (!entity.Owner.ComponentPools.ContainsKey(typeof(T)))
                entity.Owner.ComponentPools.Add(typeof(T), new Pool<T>());
            entity.Components.Add(typeof(T), entity.Owner.ComponentPools[typeof(T)].Reserve());
            entity.Owner.UpdateFilters(entity);
        }
        return ref ((Pool<T>)entity.Owner.ComponentPools[typeof(T)]).Get(entity.Components[typeof(T)]);
    }

    public static Entity Replace<T>(this Entity entity, T component)
    {
        if (entity.Components.ContainsKey(typeof(T)))
            entity.Owner.ComponentPools[typeof(T)].Set(entity.Components[typeof(T)], component);
        else
        {
            if (!entity.Owner.ComponentPools.ContainsKey(typeof(T)))
                entity.Owner.ComponentPools.Add(typeof(T), new Pool<T>());
            int id = entity.Owner.ComponentPools[typeof(T)].Reserve();
            entity.Owner.ComponentPools[typeof(T)].Set(id, component);
            entity.Components.Add(typeof(T), id);
            entity.Owner.UpdateFilters(entity);
        }
        return entity;
    }

    public static Entity Delete<T>(this Entity entity)
    {
        if (!entity.Components.ContainsKey(typeof(T))) return entity;

        int id = entity.Components[typeof(T)];
        entity.Owner.DestroyComponent(typeof(T), id);
        entity.Components.Remove(typeof(T));
        entity.Owner.UpdateFilters(entity);

        return entity;
    }
}

