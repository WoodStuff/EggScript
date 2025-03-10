﻿using EggScript.Exceptions;
using EggScript.Parsing.Nodes.Expression;
using EggScript.Parsing.Nodes.Expression.Data;
using EggScript.Parsing.Nodes.Statement;
using EggScript.Tokenization;

namespace EggScript.Parsing;

/// <summary>
/// Parses the source code's tokens into an abstract syntax tree.
/// </summary>
/// <param name="tokens">The source code's tokens, generated by the <see cref="Tokenizer"/>.</param>
internal class Parser(List<Token> tokens)
{
	private static readonly string[] booleans = ["true", "false"];
	private static readonly Dictionary<int, string[]> operators = new()
	{
		{ 0, ["+", "-"] },
		{ 1, ["*", "/"] },
	};

	/// <summary>
	/// The source code's tokens, generated by the <see cref="Tokenizer"/>.
	/// </summary>
	private List<Token> Tokens { get; } = tokens;
	/// <summary>
	/// The index of the token next in the queue.
	/// </summary>
	private int Index { get; set; } = 0;
	/// <summary>
	/// The token to be read next.
	/// </summary>
	private Token CurrentToken => Tokens[Index];

	/// <summary>
	/// Parses the <see cref="Tokens"/> into an abstract syntax tree.
	/// </summary>
	/// <returns>A list of nodes make from the tokens.</returns>
	public List<IStatementNode> Parse()
	{
		List<IStatementNode> nodes = [];
		while (!Match(TokenType.EOF, out _))
		{
			nodes.Add(ParseStatement());
		}
		return nodes;
	}

	/// <summary>
	/// Parses a statement.
	/// </summary>
	/// <returns>The node corresponding to that statement.</returns>
	/// <exception cref="EggScriptException">Thrown when a syntax error is detected.</exception>
	public IStatementNode ParseStatement()
	{
		if (!Match(TokenType.Keyword, "print")) throw new EggScriptException("Statement must start from a keyword");
		if (!Match(TokenType.Punctuation, "(")) throw new EggScriptException("( expected");

		IExpressionNode data = ParseExpression();
		PrintNode node = new(data);

		if (!Match(TokenType.Punctuation, ")")) throw new EggScriptException(") expected");
		if (!Match(TokenType.Punctuation, ";")) throw new EggScriptException("; expected");

		return node;
	}

	/// <summary>
	/// Parses an expression.
	/// </summary>
	/// <returns>The node corresponding to that expression.</returns>
	private IExpressionNode ParseExpression()
	{
		IExpressionNode data = ParseOperator();
		return data;
	}

	private IExpressionNode ParseOperator(int level = 0)
	{
		IExpressionNode data = GetLowerExpression(level);
		while (Match(TokenType.Operator, out string op, operators[level]))
		{
			IExpressionNode right = GetLowerExpression(level);
			data = new OperatorNode(data, op, right);
		}
		return data;

		IExpressionNode GetLowerExpression(int level) => level == operators.Count - 1 ? ParsePrimitive() : ParseOperator(level + 1);
	}

	/// <summary>
	/// Parses some data, such as a string or number.
	/// </summary>
	/// <returns>The node corresponding to that primitive.</returns>
	/// <exception cref="EggScriptException">Thrown when the data is not a primitive.</exception>
	private IDataNode ParsePrimitive()
	{
		Token token = Next();
		IDataNode node = token.Type switch
		{
			TokenType.String => new StringNode(token.Value),
			TokenType.Number => new NumberNode(token.Value),
			TokenType.FreeKeyword => booleans.Contains(token.Value) ? new BooleanNode(token.Value) : throw new EggScriptException("Expression expected"),
			_ => throw new EggScriptException("Expression expected"),
		};
		return node;
	}

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
	private bool Match(TokenType type, out string value, params string[] values)
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
}