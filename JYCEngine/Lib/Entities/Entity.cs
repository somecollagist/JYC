using System;
using System.Collections.Generic;

namespace JYCEngine;
/// <summary>
/// Base class of an entity.
/// </summary>
public class Entity
{
	/// <summary>
	/// Unique ID of an entity.
	/// </summary>
	public Guid ID { get; internal set; } = Guid.NewGuid();
	/// <summary>
	/// List of aliases of components used by an entity.
	/// </summary>
	public List<string> Components { get; private set; } = new();

	/// <summary>
	/// Gets the first component of a certain type on an entity
	/// </summary>
	/// <typeparam name="T">The type to get</typeparam>
	/// <returns></returns>
	public T GetFirstComponent<T>() where T : Component
    {
		foreach (var component in Components)
        {
			if (Engine.Components[component] is T)
				return (T)Engine.Components[component];
        }
		return null;
    }

	/// <summary>
	/// Registers a new component to the master component registry and this entity.
	/// </summary>
	/// <param name="alias">Alias of component to register.</param>
	/// <param name="component">Component to register.</param>
	/// <param name="useGuid">Appends this entity's GUID to the alias.</param>
	private protected void RegisterNewComponent(string alias, Component component, bool useGuid = true)
	{
		if(useGuid) alias += this.ID;
		Engine.AddComponent(alias, component);
		Components.Add(alias);
	}

	/// <summary>
	/// Registers an existing component to this entity.
	/// </summary>
	/// <param name="alias">Alias of component to register.</param>
	private protected void RegisterExistingComponent(string alias)
	{
		if (!Engine.Components.ContainsKey(alias)) throw new ComponentNonExistantException(alias);
		Components.Add(alias);
	}
}