namespace JYCEngine;

[AttributeUsage(AttributeTargets.Struct, AllowMultiple = false)]
public class RequireComponentsAttribute : Attribute
{
    public readonly Type[] Types;

    public RequireComponentsAttribute(params Type[] types)
    {
        Types = types;
    }
}

