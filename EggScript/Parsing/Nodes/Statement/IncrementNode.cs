using EggScript.Parsing.Nodes.Expression;
using EggScript.Parsing.Nodes.Expression.Data;

namespace EggScript.Parsing.Nodes.Statement;

/// <summary>
/// A node that increments a numeric variable.
/// </summary>
/// <param name="name">The name of the variable.</param>
/// <param name="data">The value to assign to the variable.</param>
internal class IncrementNode(string name, IExpressionNode value) : IStatementNode
{
	/// <summary>
	/// The name of the variable.
	/// </summary>
	public string Name { get; } = name;
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
    Value: {value}
)
""";
	}
}