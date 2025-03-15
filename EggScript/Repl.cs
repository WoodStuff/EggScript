using EggScript.Exceptions;
using EggScript.Parsing;
using EggScript.Parsing.Nodes.Expression;
using EggScript.Parsing.Nodes.Statement;
using EggScript.Runtime;
using EggScript.Tokenization;

namespace EggScript;

/// <summary>
/// The EggScript Read-Eval-Print Loop - type in expressions to see them evaluated in real time.
/// </summary>
public static class Repl
{
	/// <summary>
	/// Starts the Repl in a console window.
	/// </summary>
	public static void Start()
	{
		Console.WriteLine("EggScript Repl");

		string input;
		do
		{
			Write("> ", ConsoleColor.DarkGray);
			input = Console.ReadLine()!;
			if (input != "exit") EvaluateLine(input);
		} while (input != "exit");
	}

	/// <summary>
	/// Evaluates an EggScript line of code.
	/// </summary>
	/// <param name="input">The code.</param>
	private static void EvaluateLine(string input)
	{
		bool statement = IsStatement(input);
		if (statement) EvaluateStatement(input);
		else EvaluateExpression(input);
	}

	/// <summary>
	/// Evaluates an EggScript expression.
	/// </summary>
	/// <param name="input">The code.</param>
	private static void EvaluateExpression(string input)
	{
		try
		{
			IExpressionNode node = Parser.ParseExpression(input);
			Console.WriteLine(Interpreter.GetValue(node).Value);
		}
		catch (EggScriptException e)
		{
			WriteLine(e.Message, ConsoleColor.Red);
		}
	}

	/// <summary>
	/// Evaluates an EggScript statement.
	/// </summary>
	/// <param name="input">The code.</param>
	private static void EvaluateStatement(string input)
	{
		try
		{
			IStatementNode node = Parser.ParseStatement(input);
			Interpreter.ExecuteStatement(node);
		}
		catch (EggScriptException e)
		{
			WriteLine(e.Message, ConsoleColor.Red);
		}
	}

	/// <summary>
	/// Checks if an EggScript line of code is a statement or expression.
	/// </summary>
	/// <param name="input">The code.</param>
	/// <returns>True if the line is a statement, false if the line is an expression.</returns>
	private static bool IsStatement(string input) => Tokenizer.KeywordRegex().Match(input).Success;

	/// <summary>
	/// Writes some text with a color.
	/// </summary>
	/// <param name="value">The text to write.</param>
	/// <param name="color">The color to write it with.</param>
	private static void Write(object? value, ConsoleColor color)
	{
		ConsoleColor temp = Console.ForegroundColor;
		Console.ForegroundColor = color;
		Console.Write(value);
		Console.ForegroundColor = temp;
	}

	/// <summary>
	/// Writes some text with a color followed by a newline.
	/// </summary>
	/// <param name="value">The text to write.</param>
	/// <param name="color">The color to write it with.</param>
	private static void WriteLine(object? value, ConsoleColor color)
	{
		ConsoleColor temp = Console.ForegroundColor;
		Console.ForegroundColor = color;
		Console.WriteLine(value);
		Console.ForegroundColor = temp;
	}
}
