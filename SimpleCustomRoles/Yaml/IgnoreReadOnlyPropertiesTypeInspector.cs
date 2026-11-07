using SimpleCustomRoles.Attributes;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.TypeInspectors;

namespace SimpleCustomRoles.Yaml;

/// <summary>
/// Represent a type inspector that ignores read only properties.
/// </summary>
/// <param name="inner">The inner inspector.</param>
internal sealed class IgnoreReadOnlyPropertiesTypeInspector(ITypeInspector inner) : TypeInspectorSkeleton
{
	private readonly ITypeInspector inner = inner;

	/// <inheritdoc/>
	public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, object? container)
	{
		return inner.GetProperties(type, container).Where(p => p.CanWrite || p.GetCustomAttribute<YamlSerializePropertyAttribute>() != null);
	}
}
