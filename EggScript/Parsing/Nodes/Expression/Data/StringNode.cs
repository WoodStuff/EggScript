﻿namespace EggScript.Parsing.Nodes.Expression.Data;

/// <summary>
/// Represents a data node that holds a string.
/// </summary>
/// <param name="value">The value of the string node.</param>
public class StringNode(string value) : IDataNode
{
	/// <summary>
	/// The value of the string node.
	/// </summary>
	public string Value { get; } = value;

	object IDataNode.Value => Value;
	DataType IDataNode.Type => DataType.String;

	public static StringNode operator +(StringNode left, StringNode right) => new(left.Value + right.Value);

	public override string ToString() => $"""StringNode ({Value})""";
}