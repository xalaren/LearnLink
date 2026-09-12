namespace LearnLink.Core.Attributes;

/// <summary>
/// Attribute for controlling access with permissions
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class AccessControlAttribute : Attribute
{

}
