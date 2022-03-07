namespace JYCEngine;


/// <summary>
/// Entity Struct used for all entities
/// </summary>
public struct Entity
{
    public int ID;
    public EcsWorld Owner;
    public Dictionary<Type, int> Components;    
}

/// <summary>
/// Static Extensions class for all Entity Methods
/// </summary>
public static class EntityExtensions
{
    /// <summary>
    /// Get reference to a component on an entity
    /// </summary>
    /// <typeparam name="T">The component's Type</typeparam>
    /// <param name="entity">The target entity</param>
    /// <remarks>
    /// If the component does not exist on the entity, a blank component will be created
    /// (reserved) and returned instead. If you want to get only if it exists, use <see cref="Has{T}(Entity)"/>
    /// to check first.
    /// </remarks>
    /// <returns></returns>
    public static ref T Get<T>(this Entity entity)
    {
        if (!entity.Components.ContainsKey(typeof(T)))
        {
            if (!entity.Owner.ComponentPools.ContainsKey(typeof(T)))
                entity.Owner.RegisterNewComponent<T>();
            entity.Components.Add(typeof(T), entity.Owner.ComponentPools[typeof(T)].Reserve());
            entity.Owner.UpdateFilters(entity);
        }
        return ref ((Pool<T>)entity.Owner.ComponentPools[typeof(T)]).Get(entity.Components[typeof(T)]);
    }

    /// <summary>
    /// Add or Replace a component on an entity
    /// </summary>
    /// <typeparam name="T">The component's Type</typeparam>
    /// <param name="entity">The target entity</param>
    /// <param name="component">The component to be used</param>
    /// <remarks>
    /// Replace has two functions - if a component of the given type does
    /// not exist on an entity, the passed component will be added to the entity.
    /// If it does exist, it will be replaced with the passed entity.
    /// </remarks>
    /// <returns></returns>
    public static Entity Replace<T>(this Entity entity, T component)
    {
        if (entity.Components.ContainsKey(typeof(T)))
            entity.Owner.ComponentPools[typeof(T)].Set(entity.Components[typeof(T)], component);
        else
        {
            if (!entity.Owner.ComponentPools.ContainsKey(typeof(T)))
                entity.Owner.RegisterNewComponent<T>();
            entity.Owner.AddComponentToEntity(entity, component);
        }
        return entity;
    }

    /// <summary>
    /// Remove a component from an entity
    /// </summary>
    /// <typeparam name="T">The type of component to remove</typeparam>
    /// <param name="entity">The target entity</param>
    /// <returns></returns>
    public static Entity Delete<T>(this Entity entity)
    {
        if (!entity.Components.ContainsKey(typeof(T))) return entity;

        int id = entity.Components[typeof(T)];
        entity.Owner.DestroyComponent(typeof(T), id);
        entity.Components.Remove(typeof(T));
        entity.Owner.UpdateFilters(entity);

        return entity;
    }

    /// <summary>
    /// Check if a component has a component by type
    /// </summary>
    /// <typeparam name="T">The type to check for</typeparam>
    /// <param name="entity">The target entity</param>
    /// <returns></returns>
    public static bool Has<T>(this Entity entity) where T : struct
    {
        return entity.Components.ContainsKey(typeof(T));
    }
}

