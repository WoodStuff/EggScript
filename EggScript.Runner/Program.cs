using EggScript.Exceptions;

namespace EggScript.Runner;

internal class Program
{
	static void Main(string[] args)
	{
		if (args.Length != 1)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("Incorrect usage! Please specify a path to the EggScript file.");
			return;
		}

		try
		{
			Eggscript.Execute(args[0]);
		}
		catch (EggScriptException e)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine($"Error: {e.Message}");
		}
		Console.ReadKey();
	}
}
