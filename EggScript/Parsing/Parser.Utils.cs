using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using EggScript.Exceptions;
using EggScript.Tokenization;

namespace EggScript.Parsing;

internal partial class Parser
{
	#region Token Matching
	/// <summary>
	/// Tries to detect what token is next based on the type and advances the queue if it's correct.
	/// </summary>
	/// <param name="type">What type the next token is predicted to be.</param>
	/// <param name="value">If successful, the value of the token.</param>
	/// <returns>If the next token in the queue is of type <paramref name="type"/>.</returns>
	private bool Match(TokenType type, out string value)
	{
		value = "";
		if (CurrentToken.Type != type) return false;

		value = CurrentToken.Value;
		Index++;
		return true;
	}

	/// <summary>
	/// Tries to detect what token is next based on the type and value and advances the queue if it's correct.
	/// </summary>
	/// <param name="type">What type the next token is predicted to be.</param>
	/// <param name="value">What value the next token is predicted to have.</param>
	/// <returns>If the next token in the queue is of type <paramref name="type"/> and has value <paramref name="value"/>.</returns>
	private bool Match(TokenType type, string value)
	{
		if (CurrentToken.Type != type || CurrentToken.Value != value) return false;

		Index++;
		return true;
	}

	/// <summary>
	/// Tries to detect what token is next based on the type and values and advances the queue if it's correct.
	/// </summary>
	/// <param name="type">What type the next token is predicted to be.</param>
	/// <param name="value">If successful, the value of the token.</param>
	/// <param name="values">What values the next token is predicted to be.</param>
	/// <returns>If the next token in the queue is of type <paramref name="type"/> and has a value in <paramref name="values"/>.</returns>
	private bool Match(TokenType type, out string value, params IEnumerable<string> values)
	{
		value = "";
		if (CurrentToken.Type != type || !values.Contains(CurrentToken.Value)) return false;

		value = CurrentToken.Value;
		Index++;
		return true;
	}

	/// <summary>
	/// Gets the next token in the queue and advances the queue.
	/// </summary>
	/// <returns>The next token.</returns>
	private Token Next()
	{
		Token token = CurrentToken;
		Index++;
		return token;
	}
	#endregion

	#region Helper Methods
	/// <summary>
	/// Creates a <see cref="DataType"/> from a string.
	/// </summary>
	/// <param name="text">The string to convert to a data type.</param>
	/// <returns>The data type that the string represents.</returns>
	/// <exception cref="EggSyntaxException">Thrown if the string does not represent any <see cref="DataType"/>.</exception>
	private static DataType TypeFromString(string text) => text switch
	{
		"string" => DataType.String,
		"num" => DataType.Number,
		"bool" => DataType.Boolean,
		_ => throw new EggSyntaxException($"Invalid type {text}"),
	};

	/// <summary>
	/// Helper function to throw an error when a token is expected but the parser didn't detect it.
	/// </summary>
	/// <param name="actual">What was detected instead of the expected token.</param>
	/// <param name="expected">What was expected. Defaults to "Expression".</param>
	/// <exception cref="EggSyntaxException">Always thrown.</exception>
	[DoesNotReturn]
	[StackTraceHidden]
	private static void Throw_Expected(string actual, string expected = "Expression") => throw new EggSyntaxException($"{expected} expected - got {actual}");
	#endregion
}