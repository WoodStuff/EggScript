using EggScript.Exceptions;
using EggScript.Parsing;
using EggScript.Parsing.Nodes.Statement;
using EggScript.Runtime;
using EggScript.Tokenization;

namespace EggScript;

/// <summary>
/// Provides methods related to running EggScript files.
/// </summary>
public static class Eggscript
{
	/// <summary>
	/// Executes an .egg file.
	/// </summary>
	/// <param name="path">The path of the .egg file.</param>
	/// <exception cref="EggScriptException">Thrown when the file is not an .egg file.</exception>
	public static void Execute(string path)
	{
		if (Path.GetExtension(path) != ".egg") throw new EggFiletypeException();

		string data = File.ReadAllText(path);
		ExecuteDirect(data);
	}

	/// <summary>
	/// Directly executes EggScript code.
	/// </summary>
	/// <param name="source">The source code of the program.</param>
	public static void ExecuteDirect(string source)
	{
		var tokens = Tokenizer.Tokenize(source);

		Parser parser = new(tokens);
		List<IStatementNode> nodes = parser.Parse();

		Interpreter.Execute(nodes);
	}

	/// <summary>
	/// Displays the tokens of an .egg file.
	/// </summary>
	/// <param name="path">The path of the .egg file.</param>
	/// <exception cref="EggScriptException">Thrown when the file is not an .egg file.</exception>
	internal static void DisplayTokens(string path)
	{
		if (Path.GetExtension(path) != ".egg") throw new EggFiletypeException();

		string data = File.ReadAllText(path);
		var tokens = Tokenizer.Tokenize(data);

		Console.WriteLine(string.Join('\n', tokens));
	}

	/// <summary>
	/// Displays the AST of an .egg file.
	/// </summary>
	/// <param name="path">The path of the .egg file.</param>
	/// <exception cref="EggScriptException">Thrown when the file is not an .egg file.</exception>
	internal static void DisplayAST(string path)
	{
		if (Path.GetExtension(path) != ".egg") throw new EggFiletypeException();

		string data = File.ReadAllText(path);
		var tokens = Tokenizer.Tokenize(data);

		Parser parser = new(tokens);
		List<IStatementNode> nodes = parser.Parse();

		Console.WriteLine(string.Join('\n', nodes));
	}
}