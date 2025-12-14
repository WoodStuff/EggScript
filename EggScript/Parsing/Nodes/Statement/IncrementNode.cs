using EggScript.Parsing.Nodes.Expression;
using EggScript.Parsing.Nodes.Expression.Data;

namespace EggScript.Parsing.Nodes.Statement;

/// <summary>
/// A node that increments a variable.
/// </summary>
/// <param name="name">The name of the variable.</param>
/// <param name="op">The operator to use.</param>
/// <param name="value">The amount to increment the variable.</param>
internal class IncrementNode(string name, string op, IExpressionNode value) : IStatementNode
{
	/// <summary>
	/// The name of the variable.
	/// </summary>
	public string Name { get; } = name;
	/// <summary>
	/// The operator to use.
	/// </summary>
	public string Operator { get; } = op;
	/// <summary>
	/// The amount to increment the variable.
	/// </summary>
	public IExpressionNode Value { get; } = value;

	public override string ToString()
	{
		string value = Value.ToString()!.Replace("\n", "\n    ");
		return $"""
IncrementNode
(
    Name: {Name}
	Operator: {Operator}
    Value: {value}
)
""";
	}
}