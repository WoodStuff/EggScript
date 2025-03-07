namespace EggScript.Parsing.Nodes.Expression.Data;

/// <summary>
/// A node that represents data, like strings or numbers.
/// </summary>
public interface IDataNode : IExpressionNode
{
	/// <summary>
	/// The value of the <see cref="IDataNode"/>.
	/// </summary>
	object Value { get; }
}