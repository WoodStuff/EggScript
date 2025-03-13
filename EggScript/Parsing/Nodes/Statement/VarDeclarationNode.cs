using EggScript.Parsing.Nodes.Expression;

namespace EggScript.Parsing.Nodes.Statement;

/// <summary>
/// A node that declares a variable.
/// </summary>
/// <param name="name">The name of the declared variable.</param>
/// <param name="data">The value to assign to the variable.</param>
public class VarDeclarationNode(string name, IExpressionNode data) : IStatementNode
{
	/// <summary>
	/// The name of the declared variable.
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
VarDeclarationNode
(
    Name: {Name}
    Data: {data}
)
""";
	}
}