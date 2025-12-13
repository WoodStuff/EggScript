using EggScript.Parsing.Nodes.Expression;

namespace EggScript.Parsing.Nodes.Statement;

/// <summary>
/// A node containing multiple statements, each executed in a new scope.
/// </summary>
/// <param name="nodes">The nodes in the block.</param>
internal class BlockNode(List<IStatementNode> nodes) : IStatementNode
{
	/// <summary>
	/// The value to log.
	/// </summary>
	public List<IStatementNode> Nodes { get; } = nodes;

	public override string ToString()
	{
		string data = "".ToString()!.Replace("\n", "\n    ");
		return $"""
BlockNode
(
    Value: {data}
)
""";
	}
}