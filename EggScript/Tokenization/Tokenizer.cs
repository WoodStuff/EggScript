using System.Text.RegularExpressions;
using EggScript.Exceptions;

namespace EggScript.Tokenization;

/// <summary>
/// Converts an EggScript program's source code into tokens.
/// </summary>
internal static partial class Tokenizer
{
	/// <summary>
	/// The regex patterns for each token type.
	/// </summary>
	private static Dictionary<TokenType, Regex> Patterns { get; } = new Dictionary<TokenType, Regex> {
		{ TokenType.Keyword, KeywordRegex() },
		{ TokenType.FreeKeyword, FreeKeywordRegex() },
		{ TokenType.Whitespace, WhitespaceRegex() },
		{ TokenType.Comment, CommentRegex() },
		{ TokenType.Punctuation, PunctuationRegex() },
		{ TokenType.Operator, OperatorRegex() },
		{ TokenType.Number, NumberRegex() },
		{ TokenType.String, StringRegex() },
	};

	/// <summary>
	/// Creates tokens out of some code.
	/// </summary>
	/// <param name="source">The program's source code.</param>
	/// <returns>A list of tokens in the code.</returns>
	public static List<Token> Tokenize(string source)
	{
		int offset = 0;
		List<Token> tokens = [];

		while (offset < source.Length)
		{
			bool found = false;
			foreach (var (type, pattern) in Patterns)
			{
				Match match = pattern.Match(source[offset..]);
				if (!match.Success) continue;

				string value = match.Value;
				if (type == TokenType.String) value = value[1..^1]; // remove quotes from string

				if (IsCounted(type)) tokens.Add(new(type, value));
				offset += match.Length;
				found = true;
				break;
			}

			if (!found) throw new EggScriptException($"Unexpected character found at position {offset}: {source[offset]}");
		}

		tokens.Add(new Token(TokenType.EOF, ""));
		
		return tokens;
	}

	/// <summary>
	/// Checks if a token type actually has an effect on code execution, i.e. is not whitespace or a comment.
	/// </summary>
	/// <param name="type">The token type to check.</param>
	/// <returns>If the token type should be added to the final token list.</returns>
	private static bool IsCounted(TokenType type) => type != TokenType.Whitespace && type != TokenType.Comment;

	[GeneratedRegex(@"^print\b")]
	private static partial Regex KeywordRegex();
	[GeneratedRegex(@"^(true|false)\b")]
	private static partial Regex FreeKeywordRegex();
	[GeneratedRegex(@"^\s+")]
	private static partial Regex WhitespaceRegex();
	[GeneratedRegex(@"^\/\/.*|^\/\*[\s\S]*?\*\/")]
	private static partial Regex CommentRegex();
	[GeneratedRegex(@"^[\(\);]")]
	private static partial Regex PunctuationRegex();
	[GeneratedRegex(@"^(\+|\-|\*|/)")]
	private static partial Regex OperatorRegex();
	[GeneratedRegex(@"^\d*\.?\d+")]
	private static partial Regex NumberRegex();
	[GeneratedRegex(@"^""[^""]*""")]
	private static partial Regex StringRegex();
}