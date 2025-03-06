using EggScript;

internal class Program
{
	private static string Root => $"{AppContext.BaseDirectory}/../../..";
	private static string Path => $"{Root}/script.egg";

	private static void Main() => Eggscript.Execute(Path);
}