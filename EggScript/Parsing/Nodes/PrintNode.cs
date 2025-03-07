namespace EggScript.Parsing.Nodes;

/// <summary>
/// A node that logs something.
/// </summary>
/// <param name="value">The value to log.</param>
public class PrintNode(string value) : IStatementNode
{
	/// <summary>
	/// The value to log.
	/// </summary>
	public string Value { get; } = value;
}