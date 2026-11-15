using System.Reflection;

internal static class Versioning
{
	public static Version Version = new(typeof(Versioning).Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version ?? "0.0.0");
}
