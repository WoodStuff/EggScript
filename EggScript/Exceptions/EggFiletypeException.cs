namespace EggScript.Exceptions;

/// <summary>
/// Thrown when a file that is not an .egg file is attempted to be executed.
/// </summary>
public class EggFiletypeException : EggScriptException
{
	public EggFiletypeException() : base("File is not an .egg file, EggScript may not be executed on it.") { }
	public EggFiletypeException(string message) : base(message) { }
	public EggFiletypeException(string message, Exception inner) : base(message, inner) { }
}