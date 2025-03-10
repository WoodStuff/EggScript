﻿namespace EggScript.Parsing.Nodes.Expression.Data;

/// <summary>
/// Represents a data node that holds a boolean.
/// </summary>
public class BooleanNode : IDataNode
{
	/// <summary>
	/// The value of the boolean node.
	/// </summary>
	public bool Value { get; }

	object IDataNode.Value => Value;

	public BooleanNode(bool value) => Value = value;
	public BooleanNode(string value) => Value = bool.Parse(value);

	public override string ToString() => $"""
BooleanNode ( Value: {Value} )
""";
}