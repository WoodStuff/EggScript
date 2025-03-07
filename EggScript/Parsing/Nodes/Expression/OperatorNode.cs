namespace EggScript.Parsing.Nodes.Expression;

/// <summary>
/// Represents a binary operator node.
/// </summary>
public class OperatorNode(IExpressionNode left, string op, IExpressionNode right) : IExpressionNode
{
	/// <summary>
	/// The kind of operator, like addition or multiplication.
	/// </summary>
	public string Operator { get; } = op;
	/// <summary>
	/// The left expression.
	/// </summary>
	public IExpressionNode Left { get; } = left;
	/// <summary>
	/// The right expression.
	/// </summary>
	public IExpressionNode Right { get; } = right;
}