using System.Reflection;

internal static class Versioning
{
	public static Version Version = new(typeof(Versioning).Assembly.GetCustomAttribute<AssemblyVersionAttribute>().Version);
}
