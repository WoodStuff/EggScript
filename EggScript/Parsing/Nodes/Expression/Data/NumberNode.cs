namespace EggScript.Parsing.Nodes.Expression.Data;

/// <summary>
/// Represents a data node that holds a number.
/// </summary>
public class NumberNode : IDataNode
{
	/// <summary>
	/// The value of the number node.
	/// </summary>
	public int Value { get; }

	object IDataNode.Value => Value;

	public NumberNode(int value) => Value = value;
	public NumberNode(string value) => Value = int.Parse(value);
}