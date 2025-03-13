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
		{ 0, ["="] },
		{ 1, ["&"] },
		{ 2, ["|"] },
		{ 3, ["==", "!="] },
		{ 4, [">=", "<=", ">", "<"] },
		{ 5, ["+", "-"] },
		{ 6, ["*", "/"] },
	};
	private static readonly string[] unaryOperators = ["+", "-", "!"];

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
	/// <exception cref="EggSyntaxException">Thrown when a syntax error is detected.</exception>
	public IStatementNode ParseStatement()
	{
		IStatementNode node;

		if (Match(TokenType.Keyword, out string keyword)) node = ParseKeywordStatement(keyword);
		else
		{
			IExpressionNode expr = ParseExpression();
			node = ParseExprStatement(expr);
		}

		if (!Match(TokenType.Punctuation, ";")) throw new EggSyntaxException("; expected");

		return node;
	}

	/// <summary>
	/// Parses a statement starting from a keyword.
	/// </summary>
	/// <param name="keyword">The keyword the statement starts from.</param>
	/// <returns>The node corresponding to that statement.</returns>
	/// <exception cref="EggSyntaxException">Thrown when a syntax error is detected.</exception>
	private IStatementNode ParseKeywordStatement(string keyword)
	{
		IStatementNode node;
		switch (keyword)
		{
			case "print":
			{
				if (!Match(TokenType.Punctuation, "(")) throw new EggSyntaxException("( expected");

				IExpressionNode data = ParseExpression();
				node = new PrintNode(data);

				if (!Match(TokenType.Punctuation, ")")) throw new EggSyntaxException(") expected");
				break;
			}

			case "var":
			{
				if (!Match(TokenType.Identifier, out string name)) throw new EggSyntaxException("Identifier expected");
				if (!Match(TokenType.Operator, "=")) throw new EggSyntaxException("= expected");

				IExpressionNode data = ParseExpression();
				node = new VarDeclarationNode(name, data);

				break;
			}

			default:
				throw new EggSyntaxException("Invalid keyword");
		}
		return node;
	}

	/// <summary>
	/// Parses a statement that is an expression, like a variable assignment statement.
	/// </summary>
	/// <param name="expr">The statement's expression.</param>
	/// <returns>The node corresponding to that statement.</returns>
	/// <exception cref="EggSyntaxException">Thrown when a syntax error is detected.</exception>
	private static IStatementNode ParseExprStatement(IExpressionNode expr)
	{
		IStatementNode node;
		if (expr is OperatorNode { Left: VariableNode var } operatorNode && operatorNode.Operator == "=")
		{
			node = new VarAssignmentNode(var.Value, operatorNode.Right);
		}
		else throw new EggSyntaxException("Only keywords or assignment expressions can be used as a statement");
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

	/// <summary>
	/// Parses an expression that may contain operators.
	/// </summary>
	/// <param name="level">The operator precedence level to start at.</param>
	/// <returns>The node corresponding to that expression.</returns>
	private IExpressionNode ParseOperator(int level = 0, bool parentheses = false)
	{
		IExpressionNode data = GetLowerExpression(level);
		while (Match(TokenType.Operator, out string op, operators[level]))
		{
			IExpressionNode right = GetLowerExpression(level);
			data = new OperatorNode(data, op, right);
		}
		if (parentheses && level == 0) Match(TokenType.Punctuation, ")");
		return data;

		IExpressionNode GetLowerExpression(int level)
		{
			if (level == operators.Count - 1)
			{
				if (Match(TokenType.Punctuation, "(")) return ParseOperator(0, true);
				return ParseUnary();
			}
			return ParseOperator(level + 1);
		}
	}

	/// <summary>
	/// Parses an expression that may contain unary operators.
	/// </summary>
	/// <returns>The node corresponding to that expression.</returns>
	private IExpressionNode ParseUnary()
	{
		IExpressionNode node;
		if (Match(TokenType.Operator, out string op, unaryOperators))
		{
			IExpressionNode value = Match(TokenType.Punctuation, "(") ? ParseOperator(0, true) : ParseUnary();
			node = new UnaryOpNode(op, value);
			return node;
		}
		return ParsePrimitive();
	}

	/// <summary>
	/// Parses some data, such as a string or number.
	/// </summary>
	/// <returns>The node corresponding to that primitive.</returns>
	/// <exception cref="EggSyntaxException">Thrown when the data is not a primitive.</exception>
	private IExpressionNode ParsePrimitive()
	{
		Token token = Next();
		IExpressionNode node = token.Type switch
		{
			TokenType.String => new StringNode(token.Value),
			TokenType.Number => new NumberNode(token.Value),
			TokenType.FreeKeyword => booleans.Contains(token.Value) ? new BooleanNode(token.Value) : throw new EggSyntaxException("Expression expected"),
			TokenType.Identifier => new VariableNode(token.Value),
			_ => throw new EggSyntaxException("Expression expected"),
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
}