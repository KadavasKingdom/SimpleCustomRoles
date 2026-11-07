namespace SimpleCustomRoles.Attributes;

/// <summary>
/// Represent an attribute to able to write properties even if read only.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class YamlSerializePropertyAttribute() : Attribute;
