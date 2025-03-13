namespace EggScript.Exceptions;

/// <summary>
/// Thrown when an error related to EggScript occurs.
/// </summary>
public class EggScriptException : Exception
{
	public EggScriptException() { }
	public EggScriptException(string message) : base(message) { }
	public EggScriptException(string message, Exception inner) : base(message, inner) { }
}