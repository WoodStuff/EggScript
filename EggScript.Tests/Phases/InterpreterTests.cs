using EggScript.Parsing.Nodes.Expression;
using EggScript.Parsing.Nodes.Expression.Data;
using EggScript.Runtime;

namespace EggScript.Tests.Phases;

[TestClass]
public sealed class InterpreterTests
{
	private static EggEnvironment Env => Interpreter.Env;
	
	[TestMethod]
	public void Interpreter_HandlesPrimitivesCorrectly()
	{
		IDataNode node = new NumberNode(2);
		IDataNode result = node.GetValue(Env);
		Assert.AreEqual(new NumberNode(2), result);

		node = new StringNode("hello");
		result = node.GetValue(Env);
		Assert.AreEqual(new StringNode("hello"), result);

		node = new BooleanNode(true);
		result = node.GetValue(Env);
		Assert.AreEqual(new BooleanNode(true), result);
	}

	[TestMethod]
	public void Interpreter_CalculatesOperatorsCorrectly()
	{
		OperatorNode node = new(new NumberNode(15), "+", new NumberNode(5));
		Assert.AreEqual(new NumberNode(20), node.GetValue(Env));

		node.Operator = "-";
		Assert.AreEqual(new NumberNode(10), node.GetValue(Env));

		node.Operator = "*";
		Assert.AreEqual(new NumberNode(75), node.GetValue(Env));

		node.Operator = "/";
		Assert.AreEqual(new NumberNode(3), node.GetValue(Env));
	}

	[TestMethod]
	public void Interpreter_CalculatesUnaryOperatorsCorrectly()
	{
		UnaryOpNode node = new("-", new NumberNode(5));
		Assert.AreEqual(new NumberNode(-5), node.GetValue(Env));
	}
}