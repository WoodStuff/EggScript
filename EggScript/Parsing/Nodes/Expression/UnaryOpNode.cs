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
	/// Operators where the operand shouldn't be evaluated.
	/// Used for operators that act directly on variables, which don't require the variable's data.
	/// </summary>
	private static readonly string[] operators_dontparse = ["++"];
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
		IDataNode? operand = operators_dontparse.Contains(Operator) ? null : Operand.GetValue(env);

		switch (Operator)
		{
			case "+":
				return operand switch
				{
					NumberNode n => +n,
					_ => throw new EggRuntimeException("Invalid data types in operator"),
				};

			case "-":
				return operand switch
				{
					NumberNode n => -n,
					_ => throw new EggRuntimeException("Invalid data types in operator"),
				};

			case "!":
				return operand switch
				{
					BooleanNode n => !n,
					_ => throw new EggRuntimeException("Invalid data types in operator"),
				};

			case "++":
				switch (Operand)
				{
					case IdentifierNode n:
						IDataNode var = env.GetVariable(n.Name);
						if (var is not NumberNode num) throw new EggRuntimeException("Invalid data types in operator");

						env.ModifyVariable(n.Name, num + new NumberNode(1));
						return num;

					default:
						throw new EggRuntimeException("Operator ++ can only be applied to variables");
				}

			default:
				throw new EggRuntimeException("Invalid operator");
		}
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