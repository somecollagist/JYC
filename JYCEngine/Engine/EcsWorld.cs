using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JYCEngine;

public class EcsWorld
{
    public Pool<Entity> Entities;
    public Dictionary<Type, IPool> ComponentPools;
    public List<Filter> Filters;

    private Dictionary<Type, Type[]> _componentDependencyTable;

    public EcsWorld()
    {
        Entities = new();
        ComponentPools = new();
        Filters = new();

        _componentDependencyTable = new();
    }

    /// <summary>
    /// Update all filters at once
    /// </summary>
    /// <param name="entity">The entity that has changed</param>
    public void UpdateFilters(Entity entity)
    {
        foreach (var filter in Filters)
            filter.Update(entity);
    }

    /// <summary>
    /// Create a new entity
    /// </summary>
    /// <returns>The created entity</returns>
    public Entity CreateEntity()
    {
        int id = Entities.Reserve();

        Entity entity = new Entity();

        entity.ID = id;
        entity.Owner = this;
        entity.Components = new();

        Entities.Set(id, entity);

        return entity;
    }

    /// <summary>
    /// Copy an entity and all of its components
    /// </summary>
    /// <param name="original">The original to make a copy of</param>
    /// <returns>The new copy</returns>
    public Entity CopyEntity(Entity original)
    {
        Entity entity = CreateEntity();

        foreach (var component in original.Components)
        {
            entity.Replace(ComponentPools[component.Key].Get(component.Value));
        }

        return entity;
    }

    /// <summary>
    /// Create a filter
    /// </summary>
    /// <returns>The created filter</returns>
    public Filter CreateFilter()
    {
        Filter filter = new Filter(this);
        Filters.Add(filter);
        return filter;
    }

    /// <summary>
    /// Destroy an entity
    /// </summary>
    /// <param name="entity">The entity to destroy</param>
    /// <remarks>
    /// The entity isn't actually destroyed; it is just marked for re-use along with all of its components
    /// </remarks>
    public void DestroyEntity(Entity entity)
    {
        foreach (var component in entity.Components)
        {
            DestroyComponent(component.Key, component.Value);
        }
        Entities.Recycle(entity.ID);
    }

    /// <summary>
    /// Destroy a component
    /// </summary>
    /// <param name="type">The type of component to destroy</param>
    /// <param name="id">The id of the component to destroy</param>
    public void DestroyComponent(Type type, int id)
    {
        ComponentPools[type].Recycle(id);
    }

    /// <summary>
    /// Add a component to an entity
    /// </summary>
    /// <typeparam name="T">The type of the component</typeparam>
    /// <param name="entity">The target entity</param>
    /// <param name="component">The component to add</param>
    /// <remarks>
    /// When processing the [RequireComponents] attribute, System.Reflection is used
    /// to dynamically build and call generics from a System.Type variable at runtime.
    /// This is not ideal for performance, so this could possible be improved in the future
    /// </remarks>
    public void AddComponentToEntity<T>(Entity entity, T component)
    {
        int id = ComponentPools[typeof(T)].Reserve();
        ComponentPools[typeof(T)].Set(id, component);
        entity.Components.Add(typeof(T), id);
        var requiredComponents = _componentDependencyTable[typeof(T)];
        if (requiredComponents.Length > 0)
        {
            foreach (var type in requiredComponents)
            {
                if (!entity.Components.ContainsKey(type))
                {
                    if (!ComponentPools.ContainsKey(type))
                        CreateGeneric(GetType().GetMethod(nameof(RegisterNewComponent)), type).Invoke(this, null);
                    CreateGeneric(GetType().GetMethod(nameof(AddComponentToEntity)), type).Invoke(this, new object[] { entity, Convert.ChangeType(Activator.CreateInstance(type), type) });
                }
            }
        }
        UpdateFilters(entity);
    }

    /// <summary>
    /// Register a new (unseen) component type
    /// </summary>
    /// <typeparam name="T">The type to register</typeparam>
    public void RegisterNewComponent<T>()
    {
        ComponentPools.Add(typeof(T), new Pool<T>());

        var attributes = typeof(T).GetCustomAttributes(typeof(RequireComponentsAttribute), false);
        if (attributes.Length > 0)
            _componentDependencyTable.Add(typeof(T), ((RequireComponentsAttribute)attributes.ToArray()[0]).Types); // Extra type data from attribute
        else
            _componentDependencyTable.Add(typeof(T), new Type[0]); // Empty array
    }

    /// <summary>
    /// Internal helper method for creating a generic method given a type
    /// </summary>
    /// <param name="info">The base method</param>
    /// <param name="type">The type argument</param>
    /// <returns></returns>
    MethodInfo CreateGeneric(MethodInfo info, Type type)
    {
        return info.MakeGenericMethod(type);
    }
}