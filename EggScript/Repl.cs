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
	private static (int x, int y) ArrowPos { get; set; } = (0, 0);

	/// <summary>
	/// Starts the Repl in a console window.
	/// </summary>
	public static void Start()
	{
		Console.WriteLine("EggScript Repl");

		string input;
		do
		{
			ArrowPos = Console.GetCursorPosition();

			Write("> ", ConsoleColor.DarkGray);
			input = Console.ReadLine()!;

			if (input == "exit") break;

			bool success = EvaluateLine(input);
			SetArrowColor(success);
		} while (input != "exit");
	}

	/// <summary>
	/// Evaluates an EggScript line of code.
	/// </summary>
	/// <param name="input">The code.</param>
	/// <returns>If the evaluation succeeded.</returns>
	private static bool EvaluateLine(string input)
	{
		bool statement = IsStatement(input);
		if (statement) return EvaluateStatement(input);
		else return EvaluateExpression(input);
	}

	/// <summary>
	/// Evaluates an EggScript expression.
	/// </summary>
	/// <param name="input">The code.</param>
	/// <returns>If the evaluation succeeded.</returns>
	private static bool EvaluateExpression(string input)
	{
		try
		{
			IExpressionNode node = Parser.ParseExpression(input);
			Console.WriteLine(Interpreter.GetValue(node).Value);
			return true;
		}
		catch (EggScriptException e)
		{
			WriteLine(e.Message, ConsoleColor.Red);
			return false;
		}
	}

	/// <summary>
	/// Evaluates an EggScript statement.
	/// </summary>
	/// <param name="input">The code.</param>
	/// <returns>If the evaluation succeeded.</returns>
	private static bool EvaluateStatement(string input)
	{
		try
		{
			IStatementNode node = Parser.ParseStatement(input);
			Console.WriteLine(node);
			//Interpreter.ExecuteStatement(node);
			return true;
		}
		catch (EggScriptException e)
		{
			WriteLine(e.Message, ConsoleColor.Red);
			return false;
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

	/// <summary>
	/// Sets the last arrow's color.
	/// </summary>
	/// <param name="success">If the last line succeeded.</param>
	private static void SetArrowColor(bool success)
	{
		(int x, int y) = Console.GetCursorPosition();
		ConsoleColor color = success ? ConsoleColor.Green : ConsoleColor.Red;

		Console.SetCursorPosition(ArrowPos.x, ArrowPos.y);
		Write(">", color);

		Console.SetCursorPosition(x, y);
	}
}
