using EggScript.Parsing.Nodes.Expression;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EggScript.Parsing.Nodes.Statement;

/// <summary>
/// A conditional statement.
/// </summary>
/// <param name="condition">If this is true, the body will be executed.</param>
/// <param name="body">The statements to execute if the condition succeeds.</param>
internal class IfNode(IExpressionNode condition, List<IStatementNode> body) : IStatementNode
{
	/// <summary>
	/// If this is true, the body will be executed.
	/// </summary>
	public IExpressionNode Condition { get; } = condition;
	/// <summary>
	/// The statements to execute if the condition succeeds.
	/// </summary>
	public List<IStatementNode> Body { get; } = body;

	public override string ToString()
	{
		string condition = Condition.ToString()!.Replace("\n", "\n    ");
		string body = Body.Count == 0 ? "Empty" : string.Join('\n', Body).ToString()!.Replace("\n", "\n        ");
		return $"""
IfNode
(
    Condition: {condition}
    Body:
    (
        {body}
    )
)
""";
	}
}