using EggScript.Runtime;

namespace EggScript.Parsing.Nodes.Expression.Data;

/// <summary>
/// A node that represents data, like strings or numbers.
/// </summary>
internal interface IDataNode : IExpressionNode
{
	/// <summary>
	/// The value of the <see cref="IDataNode"/>.
	/// </summary>
	object Value { get; }
	/// <summary>
	/// The data type of the <see cref="IDataNode"/>.
	/// </summary>
	DataType Type { get; }
	/// <summary>
	/// The string representation of the <see cref="IDataNode"/>.
	/// </summary>
	string StringValue { get; }

	IDataNode IExpressionNode.GetValue(EggEnvironment env) => this;
}