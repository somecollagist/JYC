namespace JYCEngine;
public static class Extensions
{
	public static T GetComponentWithGuid<T>(this Entity entity, string alias) where T : Component
	{
		return Engine.GetComponentByAlias<T>(alias + entity.ID);
	}
}