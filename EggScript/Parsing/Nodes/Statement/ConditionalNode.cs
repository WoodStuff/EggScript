﻿using EggScript.Parsing.Nodes.Expression;

namespace EggScript.Parsing.Nodes.Statement;

/// <summary>
/// Executes a list of statements depending on if a condition is true.
/// </summary>
/// <param name="condition">If this is true, the body will be executed.</param>
/// <param name="body">The statements to execute if the condition succeeds.</param>
internal class ConditionalNode(IExpressionNode condition, List<IStatementNode> body, List<IStatementNode>? otherwise = null) : IStatementNode
{
	/// <summary>
	/// If this is true, the body will be executed.
	/// </summary>
	public IExpressionNode Condition { get; } = condition;
	/// <summary>
	/// The statements to execute if the condition succeeds.
	/// </summary>
	public List<IStatementNode> Body { get; } = body;
	/// <summary>
	/// The statements to execute (if any) if the condition fails.
	/// </summary>
	public List<IStatementNode>? Otherwise { get; } = otherwise;

	public override string ToString()
	{
		string condition = Condition.ToString()!.Replace("\n", "\n    ");
		string body = Body.Count == 0 ? "Empty" : string.Join('\n', Body).ToString()!.Replace("\n", "\n        ");
		string otherwise = Otherwise?.Count is null or 0 ? "Empty" : string.Join('\n', Otherwise).ToString()!.Replace("\n", "\n        ");
		return $"""
IfNode
(
    Condition: {condition}
    Body:
    (
        {body}
    )
    Otherwise:
    (
        {otherwise}
    )
)
""";
	}
}