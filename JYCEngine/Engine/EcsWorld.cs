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

    public void UpdateFilters(Entity entity)
    {
        foreach (var filter in Filters)
            filter.Update(entity);
    }

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

    public Entity CopyEntity(Entity original)
    {
        Entity entity = CreateEntity();

        foreach (var component in original.Components)
        {
            entity.Replace(ComponentPools[component.Key].Get(component.Value));
        }

        return entity;
    }

    public Filter CreateFilter()
    {
        Filter filter = new Filter(this);
        Filters.Add(filter);
        return filter;
    }

    public void DestroyEntity(Entity entity)
    {
        foreach (var component in entity.Components)
        {
            DestroyComponent(component.Key, component.Value);
        }
        Entities.Recycle(entity.ID);
    }

    public void DestroyComponent(Type type, int id)
    {
        ComponentPools[type].Recycle(id);
    }

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

    public void RegisterNewComponent<T>()
    {
        ComponentPools.Add(typeof(T), new Pool<T>());

        var attributes = typeof(T).GetCustomAttributes(typeof(RequireComponentsAttribute), false);
        if (attributes.Length > 0)
            _componentDependencyTable.Add(typeof(T), ((RequireComponentsAttribute)attributes.ToArray()[0]).Types); // Extra type data from attribute
        else
            _componentDependencyTable.Add(typeof(T), new Type[0]); // Empty array
    }

    MethodInfo CreateGeneric(MethodInfo info, Type type)
    {
        return info.MakeGenericMethod(type);
    }
}