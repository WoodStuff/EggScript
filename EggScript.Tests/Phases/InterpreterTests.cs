using EggScript.Parsing.Nodes.Expression;
using EggScript.Parsing.Nodes.Expression.Data;
using EggScript.Runtime;

namespace EggScript.Tests.Phases;

[TestClass]
public sealed class InterpreterTests
{
	[TestMethod]
	public void Interpreter_HandlesPrimitivesCorrectly()
	{
		IDataNode result = Interpreter.GetValue(new NumberNode(2));
		Assert.AreEqual(new NumberNode(2), result);

		result = Interpreter.GetValue(new StringNode("hello"));
		Assert.AreEqual(new StringNode("hello"), result);

		result = Interpreter.GetValue(new BooleanNode(true));
		Assert.AreEqual(new BooleanNode(true), result);
	}

	[TestMethod]
	public void Interpreter_CalculatesOperatorsCorrectly()
	{
		OperatorNode node = new(new NumberNode(15), "+", new NumberNode(5));
		Assert.AreEqual(new NumberNode(20), Interpreter.GetValue(node));

		node.Operator = "-";
		Assert.AreEqual(new NumberNode(10), Interpreter.GetValue(node));

		node.Operator = "*";
		Assert.AreEqual(new NumberNode(75), Interpreter.GetValue(node));

		node.Operator = "/";
		Assert.AreEqual(new NumberNode(3), Interpreter.GetValue(node));
	}

	[TestMethod]
	public void Interpreter_CalculatesUnaryOperatorsCorrectly()
	{
		UnaryOpNode node = new("-", new NumberNode(5));
		Assert.AreEqual(new NumberNode(-5), Interpreter.GetValue(node));
	}
}