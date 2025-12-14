using EggScript.Parsing.Nodes.Expression;

namespace EggScript.Parsing.Nodes.Statement;

/// <summary>
/// Repeatedly executes a statement block.
/// </summary>
internal class ForLoopNode(IStatementNode beginStatement, IExpressionNode condition, IStatementNode endStatement, BlockNode body) : IStatementNode
{
	/// <summary>
	/// The statement to execute at the beginning of the loop.
	/// </summary>
	IStatementNode BeginStatement { get; } = beginStatement;
	/// <summary>
	/// The condition to check after each iteration of the loop. Checked after <see cref="EndStatement"/> runs.
	/// </summary>
	IExpressionNode Condition { get; } = condition;
	/// <summary>
	/// The statement to execute at the end of each iteration of the loop.
	/// </summary>
	IStatementNode EndStatement { get; } = endStatement;
	/// <summary>
	/// The block to execute each iteration of the loop.
	/// </summary>
	BlockNode Body { get; } = body;

	public override string ToString()
	{
		string beginStatement = BeginStatement.ToString()!.Replace("\n", "\n    ");
		string condition = Condition.ToString()!.Replace("\n", "\n    ");
		string endStatement = EndStatement.ToString()!.Replace("\n", "\n    ");
		string body = Body.Count == 0 ? "Empty" : string.Join('\n', Body).ToString()!.Replace("\n", "\n    ");
		return $"""
ForLoopNode
(
    BeginStatement: {beginStatement}
    Condition: {condition}
    EndStatement: {endStatement}
    Body: {body}
)
""";
	}
}