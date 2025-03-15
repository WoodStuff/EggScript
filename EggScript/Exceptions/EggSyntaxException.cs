namespace EggScript.Exceptions;

/// <summary>
/// Thrown when the syntax of an EggScript file is invalid (the parser fails).
/// </summary>
public class EggSyntaxException : EggScriptException
{
	public EggSyntaxException() { }
	public EggSyntaxException(string message) : base(message) { }
	public EggSyntaxException(string message, Exception inner) : base(message, inner) { }
}