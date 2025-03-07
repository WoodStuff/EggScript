namespace EggScript.Parsing.Nodes.Data;

/// <summary>
/// Represents a data node that holds a number.
/// </summary>
/// <param name="value">The value of the number node.</param>
public class NumberNode(int value) : IDataNode
{
	/// <summary>
	/// The value of the number node.
	/// </summary>
	public int Value { get; } = value;
}