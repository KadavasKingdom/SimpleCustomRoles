using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace SimpleCustomRoles.Yaml;

/// <summary>
/// Represent a parser that caches the parsing events.
/// </summary>
public class EventBufferParser : IParser
{
	private readonly IEnumerator<ParsingEvent> enumerator;

	/// <summary>
	/// Initializes a new instance of the <see cref="EventBufferParser"/> class.
	/// </summary>
	/// <param name="events">The events.</param>
	public EventBufferParser(IEnumerable<ParsingEvent> events)
	{
		if (events == null)
		{
			throw new ArgumentNullException(nameof(events));
		}

		enumerator = events.GetEnumerator();
		enumerator.MoveNext();
	}

	/// <inheritdoc cref="System.Collections.IEnumerator.Current"/>
	public ParsingEvent Current => enumerator.Current;

	/// <inheritdoc cref="System.Collections.IEnumerator.MoveNext"/>
	public bool MoveNext() => enumerator.MoveNext();
}
