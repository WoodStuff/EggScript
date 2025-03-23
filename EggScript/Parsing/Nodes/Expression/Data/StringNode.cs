namespace EggScript.Parsing.Nodes.Expression.Data;

/// <summary>
/// Represents a data node that holds a string.
/// </summary>
/// <param name="value">The value of the string node.</param>
internal class StringNode(string value) : IDataNode
{
	/// <summary>
	/// The value of the string node.
	/// </summary>
	public string Value { get; } = value;

	object IDataNode.Value => Value;
	DataType IDataNode.Type => DataType.String;
	string IDataNode.StringValue => Value;

	public static StringNode operator +(StringNode left, StringNode right) => new(left.Value + right.Value);

	public override string ToString() => $"""StringNode ({Value})""";
	public override bool Equals(object? obj)
	{
		if (obj is StringNode other) return Value == other.Value;
		if (obj is string val) return Value == val;
		return false;
	}
	public override int GetHashCode() => Value.GetHashCode();

}