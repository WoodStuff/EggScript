namespace EggScript.Parsing.Nodes.Expression.Data;

/// <summary>
/// Represents a data node that holds a number.
/// </summary>
internal class NumberNode : IDataNode
{
	/// <summary>
	/// The value of the number node.
	/// </summary>
	public double Value { get; }

	object IDataNode.Value => Value;
	DataType IDataNode.Type => DataType.Number;
	string IDataNode.StringValue => Value.ToString();

	public NumberNode(double value) => Value = value;
	public NumberNode(string value) => Value = double.Parse(value);

	public static NumberNode operator +(NumberNode left, NumberNode right) => new(left.Value + right.Value);
	public static NumberNode operator -(NumberNode left, NumberNode right) => new(left.Value - right.Value);
	public static NumberNode operator *(NumberNode left, NumberNode right) => new(left.Value * right.Value);
	public static NumberNode operator /(NumberNode left, NumberNode right) => new(left.Value / right.Value);
	public static BooleanNode operator >(NumberNode left, NumberNode right) => new(left.Value > right.Value);
	public static BooleanNode operator <(NumberNode left, NumberNode right) => new(left.Value < right.Value);
	public static BooleanNode operator >=(NumberNode left, NumberNode right) => new(left.Value >= right.Value);
	public static BooleanNode operator <=(NumberNode left, NumberNode right) => new(left.Value <= right.Value);

	public static NumberNode operator +(NumberNode node) => node;
	public static NumberNode operator -(NumberNode node) => new(-node.Value);

	public override string ToString() => $"""NumberNode ({Value})""";
	public override bool Equals(object? obj)
	{
		if (obj is NumberNode other) return Value == other.Value;
		if (obj is double val) return Value == val;
		return false;
	}
	public override int GetHashCode() => Value.GetHashCode();
}