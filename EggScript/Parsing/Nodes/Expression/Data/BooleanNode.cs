namespace EggScript.Parsing.Nodes.Expression.Data;

/// <summary>
/// Represents a data node that holds a boolean.
/// </summary>
internal class BooleanNode : IDataNode
{
	/// <summary>
	/// The value of the boolean node.
	/// </summary>
	public bool Value { get; }

	object IDataNode.Value => Value;
	DataType IDataNode.Type => DataType.Boolean;
	string IDataNode.StringValue => Value ? "true" : "false";

	public BooleanNode(bool value) => Value = value;
	public BooleanNode(string value) => Value = bool.Parse(value);

	public static BooleanNode operator &(BooleanNode left, BooleanNode right) => new(left.Value && right.Value);
	public static BooleanNode operator |(BooleanNode left, BooleanNode right) => new(left.Value || right.Value);

	public static BooleanNode operator !(BooleanNode node) => new(!node.Value);

	public override string ToString() => $"""BooleanNode ({Value})""";
	public override bool Equals(object? obj)
	{
		if (obj is BooleanNode other) return Value == other.Value;
		if (obj is bool val) return Value == val;
		return false;
	}
	public override int GetHashCode() => Value.GetHashCode();
}