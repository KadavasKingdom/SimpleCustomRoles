using LabApi.Loader.Features.Yaml.CustomConverters;
using Serialization;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SimpleCustomRoles;

/// <summary>
/// Provides a parser for custom roles.
/// </summary>
public static class CustomYamlParser
{
	/// <summary>
	/// Gets the serializer instance used for serializing custom role.
	/// </summary>
	public static ISerializer Serializer { get; } = new SerializerBuilder()
		.WithEmissionPhaseObjectGraphVisitor(visitor => new CommentsObjectGraphVisitor(visitor.InnerVisitor))
		.WithTypeInspector(typeInspector => new CommentGatheringTypeInspector(typeInspector))
		.WithNamingConvention(CamelCaseNamingConvention.Instance)
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
		.WithNamingConvention(CamelCaseNamingConvention.Instance)
		.IgnoreUnmatchedProperties()
		.IgnoreFields()
		.WithTypeConverter(new CustomVectorConverter())
		.WithTypeConverter(new CustomColor32Converter())
		.WithTypeConverter(new CustomColorConverter())
		.Build();
}
