using EggScript.Parsing.Nodes.Expression;

namespace EggScript.Parsing.Nodes.Statement;

/// <summary>
/// A node that declares a variable.
/// </summary>
/// <param name="name">The name of the variable.</param>
/// <param name="data">The value to assign to the variable.</param>
/// <param name="constant">If a variable is constant, its value cannot be reassigned.</param>
internal class VarDeclarationNode(string name, IExpressionNode data, bool constant = false) : IStatementNode
{
	/// <summary>
	/// The name of the variable.
	/// </summary>
	public string Name { get; } = name;
	/// <summary>
	/// The value to assign to the variable.
	/// </summary>
	public IExpressionNode Data { get; } = data;
	/// <summary>
	/// If a variable is constant, its value cannot be reassigned.
	/// </summary>
	public bool Constant { get; } = constant;

	public override string ToString()
	{
		string data = Data.ToString()!.Replace("\n", "\n    ");
		return $"""
VarDeclarationNode
(
    Name: {Name}
    Data: {data}
    Constant: {Constant}
)
""";
	}
}