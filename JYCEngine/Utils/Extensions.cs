namespace JYCEngine;
public static class Extensions
{
	public static T GetComponentWithGuid<T>(this Entity entity, string alias) where T : Component
	{
		return Engine.GetComponentByAlias<T>(alias + entity.ID);
	}

	public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
		foreach (var element in enumerable)
        {
			action?.Invoke(element);
        }
    }
}