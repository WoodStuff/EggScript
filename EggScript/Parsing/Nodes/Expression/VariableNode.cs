namespace EggScript.Parsing.Nodes.Expression;

/// <summary>
/// Represents a variable node.
/// </summary>
/// <param name="name">The variable's name.</param>
internal class VariableNode(string name) : IExpressionNode
{
	/// <summary>
	/// The variable's name.
	/// </summary>
	public string Name { get; } = name;

	public override string ToString() => $"""VariableNode ({Name})""";
}