using EggScript.Parsing.Nodes.Expression;

namespace EggScript.Parsing.Nodes.Statement;

/// <summary>
/// A node that logs something.
/// </summary>
/// <param name="data">The value to log.</param>
public class PrintNode(IExpressionNode data) : IStatementNode
{
	/// <summary>
	/// The value to log.
	/// </summary>
	public IExpressionNode Data { get; } = data;

	public override string ToString()
	{
		string data = Data.ToString()!.Replace("\n", "\n    ");
		return $"""
PrintNode
(
    Value: {data}
)
""";
	}
}