namespace EggScript.Exceptions;

/// <summary>
/// Thrown when an error is found at runtime (the interpreter fails).
/// </summary>
public class EggRuntimeException : Exception
{
	public EggRuntimeException() { }
	public EggRuntimeException(string message) : base(message) { }
	public EggRuntimeException(string message, Exception inner) : base(message, inner) { }
}