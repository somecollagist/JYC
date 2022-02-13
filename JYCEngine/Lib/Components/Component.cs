using System;

namespace JYCEngine;
/// <summary>
/// Base class of a component.
/// </summary>
public class Component
{
	/// <summary>
	/// Unique ID of a component.
	/// </summary>
	public Guid ID { get; protected set; } = Guid.NewGuid();
	/// <summary>
	/// Active status of a component.
	/// </summary>
	/// <remarks>If this is false, functions will return nothing when using this component</remarks>
	public bool IsActive { get; set; } = true;
}