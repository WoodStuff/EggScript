using EggScript.Parsing.Nodes;
using EggScript.Parsing;
using EggScript.Runtime;
using EggScript.Tokenization;
using EggScript.Exceptions;

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
		if (Path.GetExtension(path) != ".egg") throw new EggScriptException("File is not an .ege file.");

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
		List<INode> nodes = parser.Parse();

		Interpreter.Run(nodes);
	}
}