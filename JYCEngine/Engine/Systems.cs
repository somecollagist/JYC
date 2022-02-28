using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JYCEngine;

public class Systems
{
    private EcsWorld _world;
    private List<ISystem> _systems;

    public Systems(EcsWorld world)
    {
        _systems = new();
        _world = world;
    }

    public void Init()
    {
        foreach (var system in _systems)
            system?.Init(_world);
    }

    public void Execute()
    {
        foreach (var system in _systems)
            system?.Execute();
    }

    public Systems Add(ISystem system)
    {
        _systems.Add(system);
        return this;
    }
}