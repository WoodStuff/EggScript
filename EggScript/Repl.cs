using EggScript.Exceptions;
using EggScript.Parsing.Nodes.Expression;
using EggScript.Parsing;
using EggScript.Runtime;

namespace EggScript;

public static class Repl
{
	public static void Start()
	{
		Console.WriteLine("EggScript Repl");

		string input;
		do
		{
			Write("> ", ConsoleColor.DarkGray);
			input = Console.ReadLine()!;

			IExpressionNode node;
			try
			{
				node = Parser.ParseExpression(input);
				Console.WriteLine(Interpreter.GetValue(node).Value);
			}
			catch (EggScriptException e)
			{
				WriteLine(e.Message, ConsoleColor.Red);
				continue;
			}
		} while (input != "exit");
	}

	private static void Write(object? value, ConsoleColor color)
	{
		Console.ForegroundColor = color;
		Console.Write(value);
		Console.ForegroundColor = ConsoleColor.White;
	}

	private static void WriteLine(object? value, ConsoleColor color)
	{
		Console.ForegroundColor = color;
		Console.WriteLine(value);
		Console.ForegroundColor = ConsoleColor.White;
	}
}
