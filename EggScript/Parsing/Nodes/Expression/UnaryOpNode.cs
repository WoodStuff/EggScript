using EggScript.Exceptions;
using EggScript.Parsing.Nodes.Expression.Data;
using EggScript.Runtime;

namespace EggScript.Parsing.Nodes.Expression;

/// <summary>
/// Represents a unary operator node.
/// </summary>
/// <param name="op">The kind of operator, like negation.</param>
/// <param name="value">The expression to amplify.</param>
internal class UnaryOpNode(string op, IExpressionNode value) : IExpressionNode
{
	/// <summary>
	/// The kind of operator, like negation.
	/// </summary>
	public string Operator { get; set; } = op;
	/// <summary>
	/// The expression to amplify.
	/// </summary>
	public IExpressionNode Operand { get; } = value;

	public IDataNode GetValue(EggEnvironment env)
	{
		IDataNode operand = Operand.GetValue(env);

		return Operator switch
		{
			"+" => operand switch
			{
				NumberNode n => +n,
				_ => throw new EggRuntimeException("Invalid data types in operator"),
			},
			"-" => operand switch
			{
				NumberNode n => -n,
				_ => throw new EggRuntimeException("Invalid data types in operator"),
			},
			"!" => operand switch
			{
				BooleanNode n => !n,
				_ => throw new EggRuntimeException("Invalid data types in operator"),
			},
			_ => throw new EggRuntimeException("Invalid operator"),
		};
	}

	public override string ToString()
	{
		string operand = Operand.ToString()!.Replace("\n", "\n    ");
		return $"""
UnaryOpNode
(
    Operator: {Operator}
    Value: {operand}
)
""";
	}
}