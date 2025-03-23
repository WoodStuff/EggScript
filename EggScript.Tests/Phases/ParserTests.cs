using EggScript.Exceptions;
using EggScript.Parsing;
using EggScript.Parsing.Nodes.Expression;
using EggScript.Parsing.Nodes.Expression.Data;
using EggScript.Parsing.Nodes.Statement;

namespace EggScript.Tests.Phases;

[TestClass]
public sealed class ParserTests
{
	[TestMethod]
	public void Parser_RecognizesString()
	{
		IExpressionNode node = Parser.ParseExpression(""" "hello" """);
		Assert.AreEqual(new StringNode("hello"), node);
	}

	[TestMethod]
	public void Parser_RecognizesNumber()
	{
		IExpressionNode node = Parser.ParseExpression("2");
		Assert.AreEqual(new NumberNode(2), node);
	}

	[TestMethod]
	public void Parser_RecognizesBoolean()
	{
		IExpressionNode node = Parser.ParseExpression("true");
		Assert.AreEqual(new BooleanNode(true), node);
	}

	[TestMethod]
	public void Parser_RecognizesIdentifier()
	{
		IExpressionNode node = Parser.ParseExpression("myVariable");
		Assert.AreEqual(new IdentifierNode("myVariable"), node);
	}

	[TestMethod]
	public void Parser_HandlesSimpleOperatorCorrectly()
	{
		IExpressionNode node = Parser.ParseExpression("2+3");
		Assert.IsInstanceOfType<OperatorNode>(node);

		OperatorNode op = (OperatorNode)node;
		Assert.AreEqual("+", op.Operator);
		Assert.AreEqual(new NumberNode(2), op.Left);
		Assert.AreEqual(new NumberNode(3), op.Right);
	}

	[TestMethod]
	public void Parser_HandlesUnaryOperatorCorrectly()
	{
		IExpressionNode node = Parser.ParseExpression("-3");
		Assert.IsInstanceOfType<UnaryOpNode>(node);

		UnaryOpNode op = (UnaryOpNode)node;
		Assert.AreEqual("-", op.Operator);
		Assert.AreEqual(new NumberNode(3), op.Operand);
	}

	[TestMethod]
	public void Parser_OperatorPrecedenceWorksProperly()
	{
		IExpressionNode node = Parser.ParseExpression("1+2*3");
		Assert.IsInstanceOfType<OperatorNode>(node);

		OperatorNode op = (OperatorNode)node;
		Assert.AreEqual("+", op.Operator);
		Assert.AreEqual(new NumberNode(1), op.Left);
		Assert.IsInstanceOfType<OperatorNode>(op.Right);

		OperatorNode innerOp = (OperatorNode)op.Right;
		Assert.AreEqual("*", innerOp.Operator);
		Assert.AreEqual(new NumberNode(2), innerOp.Left);
		Assert.AreEqual(new NumberNode(3), innerOp.Right);
	}

	[TestMethod]
	public void Parser_HandlesParenthesesCorrectly()
	{
		IExpressionNode node = Parser.ParseExpression("3*(2+1)");
		Assert.IsInstanceOfType<OperatorNode>(node);

		OperatorNode op = (OperatorNode)node;
		Assert.AreEqual("*", op.Operator);
		Assert.AreEqual(new NumberNode(3), op.Left);
		Assert.IsInstanceOfType<OperatorNode>(op.Right);

		OperatorNode innerOp = (OperatorNode)op.Right;
		Assert.AreEqual("+", innerOp.Operator);
		Assert.AreEqual(new NumberNode(2), innerOp.Left);
		Assert.AreEqual(new NumberNode(1), innerOp.Right);
	}

	[TestMethod]
	public void Parser_HandlesPrintNodeCorrectly()
	{
		IStatementNode node = Parser.ParseStatement("""print("hello");""");
		Assert.IsInstanceOfType<PrintNode>(node);
		Assert.AreEqual(new StringNode("hello"), ((PrintNode)node).Data);

		node = Parser.ParseStatement("""print(2);""");
		Assert.IsInstanceOfType<PrintNode>(node);
		Assert.AreEqual(new NumberNode(2), ((PrintNode)node).Data);
	}

	[TestMethod]
	public void Parser_HandlesVarDeclarationCorrectly()
	{
		IStatementNode node = Parser.ParseStatement("""num n = 2;""");
		Assert.IsInstanceOfType<VarDeclarationNode>(node);
		Assert.AreEqual("n", ((VarDeclarationNode)node).Name);
		Assert.AreEqual(new NumberNode(2), ((VarDeclarationNode)node).Data);
		Assert.AreEqual(DataType.Number, ((VarDeclarationNode)node).Type);
		Assert.IsFalse(((VarDeclarationNode)node).Constant);

		node = Parser.ParseStatement("""string text = "hello";""");
		Assert.IsInstanceOfType<VarDeclarationNode>(node);
		Assert.AreEqual("text", ((VarDeclarationNode)node).Name);
		Assert.AreEqual(new StringNode("hello"), ((VarDeclarationNode)node).Data);
		Assert.AreEqual(DataType.String, ((VarDeclarationNode)node).Type);
		Assert.IsFalse(((VarDeclarationNode)node).Constant);
	}

	[TestMethod]
	public void Parser_HandlesConstVarDeclarationCorrectly()
	{
		IStatementNode node = Parser.ParseStatement("""const num n = 2;""");
		Assert.IsInstanceOfType<VarDeclarationNode>(node);
		Assert.AreEqual("n", ((VarDeclarationNode)node).Name);
		Assert.AreEqual(new NumberNode(2), ((VarDeclarationNode)node).Data);
		Assert.AreEqual(DataType.Number, ((VarDeclarationNode)node).Type);
		Assert.IsTrue(((VarDeclarationNode)node).Constant);

		node = Parser.ParseStatement("""const string text = "hello";""");
		Assert.IsInstanceOfType<VarDeclarationNode>(node);
		Assert.AreEqual("text", ((VarDeclarationNode)node).Name);
		Assert.AreEqual(new StringNode("hello"), ((VarDeclarationNode)node).Data);
		Assert.AreEqual(DataType.String, ((VarDeclarationNode)node).Type);
		Assert.IsTrue(((VarDeclarationNode)node).Constant);
	}

	[TestMethod]
	public void Parser_HandlesVarAssignmentCorrectly()
	{
		IStatementNode node = Parser.ParseStatement("""n = 2;""");
		Assert.IsInstanceOfType<VarAssignmentNode>(node);
		Assert.AreEqual("n", ((VarAssignmentNode)node).Name);
		Assert.AreEqual(new NumberNode(2), ((VarAssignmentNode)node).Data);
	}

	[TestMethod]
	public void Parser_HandlesIfStatementCorrectly()
	{
		IStatementNode node = Parser.ParseStatement("""if (true) { print("success"); print(2); }""");
		Assert.IsInstanceOfType<ConditionalNode>(node);
		Assert.IsInstanceOfType<BooleanNode>(((ConditionalNode)node).Condition);
		Assert.AreEqual(2, ((ConditionalNode)node).Body.Count);
		Assert.IsInstanceOfType<PrintNode>(((ConditionalNode)node).Body[0]);
		Assert.IsInstanceOfType<PrintNode>(((ConditionalNode)node).Body[1]);
	}

	[TestMethod]
	public void Parser_WhenMissingSemicolonAfterStatement_ThrowsError()
	{
		Assert.ThrowsException<EggSyntaxException>(() => Parser.ParseStatement("""print("hello")"""));
	}
}
