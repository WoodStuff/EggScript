namespace EggScript.Tokenization;

/// <summary>
/// An individual part of the code.
/// </summary>
/// <param name="type">The category of the token.</param>
/// <param name="value">The value of the token.</param>
internal class Token(TokenType type, string value)
{
	/// <summary>
	/// The category of the token.
	/// </summary>
	public TokenType Type { get; } = type;
	/// <summary>
	/// The token's value in the code.
	/// </summary>
	public string Value { get; } = value;

	/// <summary>
	/// How many characters the token takes up, excluding any whitespace before or after it.
	/// </summary>
	public int Length => Value.Length;

	public override string ToString() => $"{Type} {Value}";
	public override bool Equals(object? obj) => obj is Token other && Type == other.Type && Value == other.Value;
	public override int GetHashCode() => HashCode.Combine(Type, Value);
}