using EggScript.Exceptions;
using EggScript.Parsing.Nodes.Expression.Data;
using EggScript.Runtime;

namespace EggScript.Parsing.Nodes.Expression;

/// <summary>
/// Represents a binary operator node.
/// </summary>
/// <param name="left">The left expression.</param>
/// <param name="op">The kind of operator, like addition or multiplication.</param>
/// <param name="right">The right expression.</param>
internal class OperatorNode(IExpressionNode left, string op, IExpressionNode right) : IExpressionNode
{
	/// <summary>
	/// Operators where the left side shouldn't be evaluated.
	/// Used for operators that act directly on variables, which don't require the variable's data.
	/// </summary>
	private static readonly string[] operators_dontparseleft = ["="];

	/// <summary>
	/// The kind of operator, like addition or multiplication.
	/// </summary>
	public string Operator { get; set; } = op;
	/// <summary>
	/// The left expression.
	/// </summary>
	public IExpressionNode Left { get; } = left;
	/// <summary>
	/// The right expression.
	/// </summary>
	public IExpressionNode Right { get; } = right;

	public IDataNode GetValue(EggEnvironment env)
	{
		IDataNode? left = operators_dontparseleft.Contains(Operator) ? null : Left.GetValue(env);
		IDataNode right = Right.GetValue(env);

		switch (Operator)
		{
			case "+":
				return (left, right) switch
				{
					(NumberNode l, NumberNode r) => l + r,
					(StringNode l, StringNode r) => l + r,
					_ => throw new EggRuntimeException("Invalid data types in operator"),
				};

			case "-":
				return (left, right) switch
				{
					(NumberNode l, NumberNode r) => l - r,
					_ => throw new EggRuntimeException("Invalid data types in operator"),
				};

			case "*":
				return (left, right) switch
				{
					(NumberNode l, NumberNode r) => l * r,
					_ => throw new EggRuntimeException("Invalid data types in operator"),
				};

			case "/":
				return (left, right) switch
				{
					(NumberNode l, NumberNode r) => l / r,
					_ => throw new EggRuntimeException("Invalid data types in operator"),
				};

			case "&":
				return (left, right) switch
				{
					(BooleanNode l, BooleanNode r) => l & r,
					_ => throw new EggRuntimeException("Invalid data types in operator"),
				};

			case "|":
				return (left, right) switch
				{
					(BooleanNode l, BooleanNode r) => l | r,
					_ => throw new EggRuntimeException("Invalid data types in operator"),
				};

			case "==":
				return (left, right) switch
				{
					(StringNode l, StringNode r) => new BooleanNode(l.Value == r.Value),
					(NumberNode l, NumberNode r) => new BooleanNode(l.Value == r.Value),
					(BooleanNode l, BooleanNode r) => new BooleanNode(l.Value == r.Value),
					_ => throw new EggRuntimeException("Invalid data types in operator"),
				};

			case "!=":
				return (left, right) switch
				{
					(StringNode l, StringNode r) => new BooleanNode(l.Value != r.Value),
					(NumberNode l, NumberNode r) => new BooleanNode(l.Value != r.Value),
					(BooleanNode l, BooleanNode r) => new BooleanNode(l.Value != r.Value),
					_ => throw new EggRuntimeException("Invalid data types in operator"),
				};

			case ">":
				return (left, right) switch
				{
					(NumberNode l, NumberNode r) => l > r,
					_ => throw new EggRuntimeException("Invalid data types in operator"),
				};

			case "<":
				return (left, right) switch
				{
					(NumberNode l, NumberNode r) => l < r,
					_ => throw new EggRuntimeException("Invalid data types in operator"),
				};

			case ">=":
				return (left, right) switch
				{
					(NumberNode l, NumberNode r) => l >= r,
					_ => throw new EggRuntimeException("Invalid data types in operator"),
				};

			case "<=":
				return (left, right) switch
				{
					(NumberNode l, NumberNode r) => l <= r,
					_ => throw new EggRuntimeException("Invalid data types in operator"),
				};

			case "=":
			case "+=":
			case "*=":
				switch (Left)
				{
					case IdentifierNode:
						Interpreter.ExecuteStatement(Parser.ParseExprStatement(this));
						return right;

					default:
						throw new EggRuntimeException("Invalid data types in operator");
				}

			default:
				throw new EggRuntimeException("Invalid operator");
		}
	}

	public override string ToString()
	{
		string left = Left.ToString()!.Replace("\n", "\n    ");
		string right = Right.ToString()!.Replace("\n", "\n    ");
		return $"""
OperatorNode
(
    Operator: {Operator}
    Left: {left}
    Right: {right}
)
""";
	}
}