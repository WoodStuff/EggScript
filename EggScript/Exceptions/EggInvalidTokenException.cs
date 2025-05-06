namespace EggScript.Exceptions;

/// <summary>
/// Thrown when an invalid token is detected in an EggScript file (the tokenizer fails).
/// </summary>
public class EggInvalidTokenException(string message, int position, char character) : EggScriptException(message)
{
	/// <summary>
	/// The position of the character.
	/// </summary>
	public int Position { get; } = position;
	/// <summary>
	/// The character that is invalid.
	/// </summary>
	public char Character { get; } = character;

	public override string Message => $"{base.Message} (position {Position}, character {Character})";
}