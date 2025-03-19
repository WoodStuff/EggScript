using System.Diagnostics.CodeAnalysis;
using EggScript.Parsing.Nodes.Expression;

namespace EggScript.Parsing.Nodes.Statement;

/// <summary>
/// A node that declares a variable.
/// </summary>
/// <param name="name">The name of the variable.</param>
/// <param name="type">The variable's type. This cannot be changed later.</param>
/// <param name="data">The value to assign to the variable.</param>
/// <param name="constant">If a variable is constant, its value cannot be reassigned.</param>
internal class VarDeclarationNode(string name, DataType type, IExpressionNode data, bool constant = false) : IStatementNode
{
	/// <summary>
	/// The name of the variable.
	/// </summary>
	public string Name { get; } = name;
	/// <summary>
	/// The variable's type. This cannot be changed later.
	/// </summary>
	public DataType Type { get; } = type;
	/// <summary>
	/// The value to assign to the variable.
	/// </summary>
	public IExpressionNode Data { get; } = data;
	/// <summary>
	/// If a variable is constant, its value cannot be reassigned.
	/// </summary>
	public bool Constant { get; } = constant;

	/// <summary>
	/// If the variable is initialized. An uninitialized variable is defined like: string txt;
	/// </summary>
	[MemberNotNullWhen(true, nameof(Data))]
	public bool Initialized => Data != null;

	public override string ToString()
	{
		string data = Data.ToString()!.Replace("\n", "\n    ");
		string declarationInfo = Initialized ? $"""
    Data: {data}
    Constant: {Constant}
""" : "Uninitialized";
		return $"""
VarDeclarationNode
(
    Name: {Name}
    Type: {Type}
{declarationInfo}
)
""";
	}
}