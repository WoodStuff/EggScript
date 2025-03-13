namespace EggScript.Parsing.Nodes.Expression.Data;

/// <summary>
/// Represents a variable node.
/// </summary>
/// <param name="value">The variable's name.</param>
public class VariableNode(string value) : IDataNode
{
	/// <summary>
	/// The variable's name.
	/// </summary>
	public string Value { get; } = value;

	object IDataNode.Value => Value;

	public override string ToString() => $"""VariableNode ({Value})""";
}