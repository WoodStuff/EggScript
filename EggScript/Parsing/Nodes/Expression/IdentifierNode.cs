namespace EggScript.Parsing.Nodes.Expression;

/// <summary>
/// Represents an identifier node.
/// </summary>
/// <param name="name">The identifier's name.</param>
internal class IdentifierNode(string name) : IExpressionNode
{
	/// <summary>
	/// The identifier's name.
	/// </summary>
	public string Name { get; } = name;

	public override string ToString() => $"""IdentifierNode ({Name})""";
}