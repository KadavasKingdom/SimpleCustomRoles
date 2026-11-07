using SimpleCustomRoles.Roles;
using System.Diagnostics.CodeAnalysis;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace SimpleCustomRoles.Yaml;

/// <summary>
/// Represent a custom role node deserilaizer.
/// </summary>
internal sealed class CustomRoleNodeDeserializer : INodeDeserializer
{
	/// <summary>
	/// Gets the node deserializer.
	/// </summary>
	internal static CustomRoleNodeDeserializer NodeDeserializer { get; } = new();

	private static void PopulateEventList(ref List<ParsingEvent?> events, IParser reader)
	{
		int depth = 1;
		while (depth > 0 && reader.MoveNext())
		{
			events.Add(reader.Current);
			if (reader.Current is MappingStart || reader.Current is SequenceStart)
			{
				depth++;
			}
			else if (reader.Current is MappingEnd || reader.Current is SequenceEnd)
			{
				depth--;
			}
		}
	}

	private static bool TryFindType(List<ParsingEvent?> events, out string discriminatorValue, [NotNullWhen(true)] out Type? targetType)
	{
		discriminatorValue = string.Empty;
		for (int i = 0; i < events.Count - 1; i++)
		{
			if (events[i] is not Scalar key)
			{
				continue;
			}

			if (key.Value != "ParserClassName")
			{
				continue;
			}

			if (events[i + 1] is Scalar val)
			{
				discriminatorValue = val.Value;
				break;
			}
		}

		return CustomRoles.TypeMapping.TryGetValue(discriminatorValue, out targetType);
	}

	/// <inheritdoc/>
	public bool Deserialize(IParser reader, Type expectedType, Func<IParser, Type, object?> nestedObjectDeserializer, out object? value)
	{
		if (expectedType != typeof(CustomRoleBase))
		{
			value = null;
			return false;
		}

		List<ParsingEvent?> events = [];
		events.Add(reader.Current);

		PopulateEventList(ref events, reader);

		reader.MoveNext();

		if (!TryFindType(events, out string discriminatorValue, out Type? targetType))
		{
			throw new InvalidOperationException($"Unknown ParserClassName: '{discriminatorValue}'");
		}

		EventBufferParser replayParser = new(events.Where(x => x is not null).Select(x => x!));
		value = nestedObjectDeserializer(replayParser, targetType);

		return true;
	}
}
