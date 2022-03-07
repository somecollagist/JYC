namespace JYCEngine;

/// <summary>
/// A class for filtering entities
/// </summary>
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

    /// <summary>
    /// Update the filter for an entity
    /// </summary>
    /// <param name="entity">The entity to update</param>
    /// <remarks>
    /// Filters are updated per entity, whenever a change/update is made to any of the entity's components
    /// </remarks>
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

    /// <summary>
    /// Fancy IEnumerable getter for the filter's results
    /// Can be used in a foreach loop
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Entity> Matches()
    {
        foreach (var match in _matches)
            yield return _world.Entities.Get(match);
    }

    /// <summary>
    /// Add a requirement for a component (whitelist)
    /// </summary>
    /// <typeparam name="T">The required type</typeparam>
    /// <returns></returns>
    public Filter Require<T>()
    {
        _require.Add(typeof(T));
        return this;
    }

    /// <summary>
    /// Add an exclusion for a component (blacklist)
    /// </summary>
    /// <typeparam name="T">The excluded type</typeparam>
    /// <returns></returns>
    public Filter Exclude<T>()
    {
        _exclude.Add(typeof(T));
        return this;
    }
}
