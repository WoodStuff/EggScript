using EggScript.Parsing.Nodes.Data;

namespace EggScript.Parsing.Nodes.Statement;

/// <summary>
/// A node that logs something.
/// </summary>
/// <param name="data">The value to log.</param>
public class PrintNode(IDataNode data) : IStatementNode
{
	/// <summary>
	/// The value to log.
	/// </summary>
	public IDataNode Data { get; } = data;
}