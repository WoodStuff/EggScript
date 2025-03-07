namespace EggScript.Tokenization;

/// <summary>
/// Specifies a token type.
/// </summary>
internal enum TokenType
{
	/// <summary>
	/// A reserved keyword, like print.
	/// </summary>
	Keyword,
	/// <summary>
	/// Characters that don't matter, like spaces, tabs, or newlines. Excluded from the tokenization.
	/// </summary>
	Whitespace,
	/// <summary>
	/// Punctuation that has special meaning, like brackets.
	/// </summary>
	Punctuation,
	/// <summary>
	/// An operator, such as + or *.
	/// </summary>
	Operator,
	/// <summary>
	/// A number, like 7, -823 or 12.5.
	/// </summary>
	Number,
	/// <summary>
	/// A string, enclosed by double quotes, like "hello".
	/// </summary>
	String,
	/// <summary>
	/// A code comment, which ignores anything written after it. <para />
	/// Block comment: Includes // and everything after it. <para />
	/// Multiline comment: Includes /* */ and everything between those.
	/// </summary>
	Comment,
	/// <summary>
	/// The end of the source code. Always has a value of empty string.
	/// </summary>
	EOF,
}