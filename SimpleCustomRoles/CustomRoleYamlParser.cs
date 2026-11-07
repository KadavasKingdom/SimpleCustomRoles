using LabApi.Loader.Features.Yaml.CustomConverters;
using Serialization;
using SimpleCustomRoles.Yaml;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SimpleCustomRoles;

/// <summary>
/// Provides a parser for custom roles.
/// </summary>
public static class CustomRoleYamlParser
{
	/// <summary>
	/// Gets the serializer instance used for serializing custom role.
	/// </summary>
	public static ISerializer Serializer { get; } = new SerializerBuilder()
		.WithEmissionPhaseObjectGraphVisitor(visitor => new CommentsObjectGraphVisitor(visitor.InnerVisitor))
		.WithTypeInspector(typeInspector => new CommentGatheringTypeInspector(typeInspector))
		.WithTypeInspector(typeInspector => new IgnoreReadOnlyPropertiesTypeInspector(typeInspector))
		.WithNamingConvention(PascalCaseNamingConvention.Instance)
		.DisableAliases()
		.IgnoreFields()
		.WithTypeConverter(new CustomVectorConverter())
		.WithTypeConverter(new CustomColor32Converter())
		.WithTypeConverter(new CustomColorConverter())
		.Build();

	/// <summary>
	/// Gets the deserializer instance used for deserializing custom role.
	/// </summary>
	public static IDeserializer Deserializer { get; } = new DeserializerBuilder()
		.WithNamingConvention(PascalCaseNamingConvention.Instance)
		.IgnoreUnmatchedProperties()
		.IgnoreFields()
		.WithTypeConverter(new CustomVectorConverter())
		.WithTypeConverter(new CustomColor32Converter())
		.WithTypeConverter(new CustomColorConverter())
		.WithNodeDeserializer(CustomRoleNodeDeserializer.NodeDeserializer)
		.Build();
}
