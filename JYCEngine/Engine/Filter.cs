using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JYCEngine;

public class Filter
{
    private EcsWorld _world;

    private List<Type> _require;
    private List<Type> _exclude;

    private HashSet<int> _matches;

    public Filter(EcsWorld world)
    {
        _world = world;

        _require = new();
        _exclude = new();

        _matches = new();
    }

    public void Update(Entity entity)
    {
        int requireCount = 0;
        foreach ((var type, var id) in entity.Components)
        {
            foreach (var exlude in _exclude)
            {
                if (type == exlude)
                {
                    _matches.Remove(entity.ID);
                    return;
                }
            }
            foreach (var require in _require)
            {
                if (type == require) requireCount++;
            }
        }
        if (requireCount < _require.Count)
        {
            _matches.Remove(entity.ID);
            return;
        }
        _matches.Add(entity.ID);
    }

    public IEnumerable<Entity> Matches()
    {
        foreach (var match in _matches)
            yield return _world.Entities.Get(match);
    }

    public Filter Require<T>()
    {
        _require.Add(typeof(T));
        return this;
    }

    public Filter Exclude<T>()
    {
        _exclude.Add(typeof(T));
        return this;
    }
}
