namespace JYCEngine;

public class ComponentNonExistantException : Exception
{
	public ComponentNonExistantException(string name) : base($"The component '{name}' does not exist.") { }
}

public class ComponentNotRegisteredException : Exception
{
	public ComponentNotRegisteredException(string name) : base($"The component '{name}' has not been registered on this object.") { }
}