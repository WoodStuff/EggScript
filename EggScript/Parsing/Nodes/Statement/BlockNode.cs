namespace EggScript.Parsing.Nodes.Statement;

/// <summary>
/// A node containing multiple statements, each executed in a new scope.
/// </summary>
internal class BlockNode : IStatementNode
{
	public BlockNode() => Nodes = [];
	public BlockNode(IStatementNode node) => Nodes = [node];
	public BlockNode(List<IStatementNode> nodes) => Nodes = nodes;

	/// <summary>
	/// The nodes in the block.
	/// </summary>
	public List<IStatementNode> Nodes { get; }

	public IStatementNode this[int index]
	{
		get => Nodes[index];
		set => Nodes[index] = value;
	}

	/// <summary>
	/// The amount of nodes in this block.
	/// </summary>
	public int Count => Nodes.Count;

	/// <summary>
	/// Adds a statement node to the block.
	/// </summary>
	/// <param name="node">The statement to add.</param>
	public void Add(IStatementNode node) => Nodes.Add(node);

	public override string ToString()
	{
		string data = string.Join('\n', Nodes).ToString()!.Replace("\n", "\n    ");
		return $"""
BlockNode ({Count})
(
    {data}
)
""";
	}
}