using EggScript.Parsing.Nodes.Data;

namespace EggScript.Parsing.Nodes.Statement;

/// <summary>
/// A node that logs something.
/// </summary>
/// <param name="text">The text to log.</param>
public class PrintNode(StringNode text) : IStatementNode
{
	/// <summary>
	/// The value to log.
	/// </summary>
	public StringNode Text { get; } = text;
}