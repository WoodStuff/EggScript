using EggScript.Parsing.Nodes.Expression.Data;

namespace EggScript.Parsing;

/// <summary>
/// Variable data types.
/// </summary>
internal enum DataType
{
	/// <summary>
	/// A string value. Node: <see cref="StringNode"/>.
	/// </summary>
	String,
	/// <summary>
	/// A number value. Node: <see cref="NumberNode"/>.
	/// </summary>
	Number,
	/// <summary>
	/// A boolean value. Node: <see cref="BooleanNode"/>.
	/// </summary>
	Boolean,
}