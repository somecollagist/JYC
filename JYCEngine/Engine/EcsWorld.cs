using System;
using System.Collections.Generic;
using System.Linq;
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
                    AddComponentToEntity(entity, Convert.ChangeType(Activator.CreateInstance(type), type), type);
            }
        }
        UpdateFilters(entity);
    }

    public void AddComponentToEntity(Entity entity, object component, Type componentType)
    {
        int id = ComponentPools[componentType].Reserve();
        ComponentPools[componentType].Set(id, component);
        entity.Components.Add(componentType, id);
        var requiredComponents = _componentDependencyTable[componentType];
        if (requiredComponents.Length > 0)
        {
            foreach (var type in requiredComponents)
            {
                if (!entity.Components.ContainsKey(type))
                    AddComponentToEntity(entity, Convert.ChangeType(Activator.CreateInstance(type), type));
            }
        }
        UpdateFilters(entity);
    }

    public void RegisterNewComponent<T>()
    {
        ComponentPools.Add(typeof(T), new Pool<T>());

        Console.WriteLine($"Registered {typeof(T)}");
        var attributes = typeof(T).GetCustomAttributes(typeof(RequireComponentsAttribute), false);
        if (attributes.Length > 0)
            _componentDependencyTable.Add(typeof(T), ((RequireComponentsAttribute)attributes.ToArray()[0]).Types); // Extra type data from attribute
        else
            _componentDependencyTable.Add(typeof(T), new Type[0]); // Empty array
        foreach (var type in _componentDependencyTable[typeof(T)])
        {
            Console.WriteLine($" - {type}");
        }
    }
}