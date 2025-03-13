namespace EggScript.Parsing.Nodes.Expression.Data;

/// <summary>
/// Represents a variable node.
/// </summary>
/// <param name="name">The variable's name.</param>
public class VariableNode(string name) : IExpressionNode
{
	/// <summary>
	/// The variable's name.
	/// </summary>
	public string Name { get; } = name;

	public override string ToString() => $"""VariableNode ({Name})""";
}