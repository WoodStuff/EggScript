namespace EggScript.Parsing.Nodes.Data;

/// <summary>
/// A node that represents data, like strings or numbers.
/// </summary>
public interface IDataNode : INode
{
	/// <summary>
	/// The value of the <see cref="IDataNode"/>.
	/// </summary>
	object Value { get; }
}