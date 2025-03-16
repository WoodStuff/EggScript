using EggScript.Parsing.Nodes.Expression;

namespace EggScript.Parsing.Nodes.Statement;

/// <summary>
/// A node that modifies a variable.
/// </summary>
/// <param name="name">The name of the variable.</param>
/// <param name="data">The value to assign to the variable.</param>
internal class VarAssignmentNode(string name, IExpressionNode data) : IStatementNode
{
	/// <summary>
	/// The name of the variable.
	/// </summary>
	public string Name { get; } = name;
	/// <summary>
	/// The value to assign to the variable.
	/// </summary>
	public IExpressionNode Data { get; } = data;

	public override string ToString()
	{
		string data = Data.ToString()!.Replace("\n", "\n    ");
		return $"""
VarAssignmentNode
(
    Name: {Name}
    Data: {data}
)
""";
	}
}