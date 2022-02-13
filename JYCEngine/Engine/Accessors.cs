namespace JYCEngine;

public static partial class Engine
{
	/// <summary>
	/// Adds a component to the master component registry.
	/// </summary>
	/// <param name="alias">Alias to register the component by.</param>
	/// <param name="component">Component to be registered.</param>
	public static void AddComponent(string alias, Component component)
	{
		Components.Add(alias, component);
	}

	/// <summary>
	/// Removes a component from the master component registry.
	/// </summary>
	/// <param name="alias">Alias of component to be removed.</param>
	public static void RemoveComponent(string alias)
	{
		Components.Remove(alias);
	}

	/// <summary>
	/// Returns the component from the master component registry with the specified alias.
	/// </summary>
	/// <param name="alias">Alias of the desired component.</param>
	/// <returns>The component with the specified alias.</returns>
	public static Component GetComponentByAlias(string alias) => GetComponentByAlias<Component>(alias);

	/// <summary>
	/// Returns the component from the master component registry with the specified alias.
	/// </summary>
	/// <param name="alias">Alias of the desired component.</param>
	/// <typeparam name="T">The type of the desired component.</typeparam>
	/// <returns>The component with the specified alias.</returns>
	public static T GetComponentByAlias<T>(string alias) where T : Component
	{
		return (T)Components[alias];
	}

	/// <summary>
	/// Returns the first component from the master component registry whose alias satisfies a predicate.
	/// </summary>
	/// <param name="predicate">A function to test each element for a condition.</param>
	/// <returns>The first component whose alias satisfies the given predicate.</returns>
	public static Component GetComponentByPredicate(Func<string, bool> predicate) => GetComponentByPredicate<Component>(predicate);

	/// <summary>
	/// Returns the first component from the master component registry whose alias satisfies a predicate.
	/// </summary>
	/// <param name="predicate">A function to test each element for a condition.</param>
	/// <typeparam name="T">The type of the desired component.</typeparam>
	/// <returns>The first component whose alias satisfies the given predicate.</returns>
	public static T GetComponentByPredicate<T>(Func<string, bool> predicate) where T : Component
	{
		return (T)Components.First(t => predicate(t.Key)).Value;
	}

	/// <summary>
	/// Returns the first component from the master component registry which satisfies a predicate.
	/// </summary>
	/// <param name="predicate">A function to test each element for a condition.</param>
	/// <returns>The first component which satisfies the given predicate.</returns>
	public static Component GetComponentByPredicate(Func<Component, bool> predicate) => GetComponentByPredicate<Component>(predicate);

	/// <summary>
	/// Returns the first component from the master component registry which satisfies a predicate.
	/// </summary>
	/// <param name="predicate">A function to test each element for a condition.</param>
	/// <typeparam name="T">The type of the desired component.</typeparam>
	/// <returns>The first component which satisfies the given predicate.</returns>
	public static T GetComponentByPredicate<T>(Func<Component, bool> predicate) where T : Component
	{
		return (T)Components.First(t => predicate(t.Value)).Value;
	}
}