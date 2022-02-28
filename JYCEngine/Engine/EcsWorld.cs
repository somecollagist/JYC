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

    public EcsWorld()
    {
        Entities = new();
        ComponentPools = new();
        Filters = new();
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
}