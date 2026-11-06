using System.Reflection;

/// <summary>
/// Provides a version from the assembly.
/// </summary>
internal static class Versioning
{
	/// <summary>
	/// The version of this assembly.
	/// </summary>
	public static readonly Version Version = new(typeof(Versioning).Assembly.GetCustomAttribute<AssemblyVersionAttribute>().Version);
}
