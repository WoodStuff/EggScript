﻿namespace EggScript.Parsing.Nodes.Expression;

/// <summary>
/// Represents a unary operator node.
/// </summary>
/// <param name="op">The kind of operator, like negation.</param>
/// <param name="value">The expression to amplify.</param>
internal class UnaryOpNode(string op, IExpressionNode value) : IExpressionNode
{
	/// <summary>
	/// The kind of operator, like negation.
	/// </summary>
	public string Operator { get; set; } = op;
	/// <summary>
	/// The expression to amplify.
	/// </summary>
	public IExpressionNode Operand { get; } = value;

	public override string ToString()
	{
		string operand = Operand.ToString()!.Replace("\n", "\n    ");
		return $"""
UnaryOpNode
(
    Operator: {Operator}
    Value: {operand}
)
""";
	}
}