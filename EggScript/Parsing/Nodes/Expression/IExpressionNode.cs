using EggScript.Parsing.Nodes.Expression.Data;
using EggScript.Runtime;

namespace EggScript.Parsing.Nodes.Expression;

/// <summary>
/// An expression that evaluates to some data.
/// </summary>
internal interface IExpressionNode : INode
{
	/// <summary>
	/// Gets the calculated runtime value of this <see cref="IExpressionNode"/>.
	/// </summary>
	/// <param name="env">The runtime environment.</param>
	/// <returns>The <see cref="IDataNode"/> this <see cref="IExpressionNode"/> evaluates to.</returns>
	IDataNode GetValue(EggEnvironment env);
}