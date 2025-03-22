namespace EggScript.Tests;

[TestClass]
public sealed class ExpressionTests
{
	StringWriter output = new();
	static string Br => Environment.NewLine;

	#region Init/Cleanup
	[TestInitialize]
	public void Initialize()
	{
		output = new StringWriter();
		Console.SetOut(output);
	}

	[TestCleanup]
	public void Cleanup()
	{
		Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
	}
	#endregion

	#region Binary Operators
	[TestMethod]
	public void BinaryOperator_NumberAddition_CalculatesCorrectResult()
	{
		string source = """print(15 + 5);""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"20{Br}", output.ToString());
	}

	[TestMethod]
	public void BinaryOperator_NumberSubtraction_CalculatesCorrectResult()
	{
		string source = """print(15 - 5);""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"10{Br}", output.ToString());
	}

	[TestMethod]
	public void BinaryOperator_NumberMultiplication_CalculatesCorrectResult()
	{
		string source = """print(15 * 5);""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"75{Br}", output.ToString());
	}

	[TestMethod]
	public void BinaryOperator_NumberDivision_CalculatesCorrectResult()
	{
		string source = """print(15 / 5);""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"3{Br}", output.ToString());
	}

	[TestMethod]
	public void BinaryOperator_StringAddition_ConcatenatesStrings()
	{
		string source = """print("left" + "right");""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"leftright{Br}", output.ToString());
	}

	[TestMethod]
	public void BinaryOperator_BooleanOr_MergesBooleansProperly()
	{
		string source = """
print(false | false);
print(false | true);
print(true | true);
""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"False{Br}True{Br}True{Br}", output.ToString());
	}

	[TestMethod]
	public void BinaryOperator_BooleanAnd_MergesBooleansProperly()
	{
		string source = """
print(false & false);
print(false & true);
print(true & true);
""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"False{Br}False{Br}True{Br}", output.ToString());
	}

	[TestMethod]
	public void BinaryOperator_CanBeChained()
	{
		string source = """print(7 + 2 + 5 + 1);""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"15{Br}", output.ToString());
	}

	[TestMethod]
	public void BinaryOperator_FollowsPrecedenceRules()
	{
		string source = """
print(5 * 6 + 2);
print(2 + 5 * 6);
""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"32{Br}32{Br}", output.ToString());
	}
	#endregion

	#region Unary Operators
	[TestMethod]
	public void UnaryOperator_NumberPlus_DoesNotChangeNumber()
	{
		string source = """print(+15);""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"15{Br}", output.ToString());
	}

	[TestMethod]
	public void UnaryOperator_NumberMinus_NegatesNumber()
	{
		string source = """print(-15);""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"-15{Br}", output.ToString());
	}

	[TestMethod]
	public void UnaryOperator_BooleanNegate_NegatesBoolean()
	{
		string source = """
print(!true);
print(!false);
""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"False{Br}True{Br}", output.ToString());
	}

	[TestMethod]
	public void UnaryOperator_CanBeChained()
	{
		string source = """print(--15);""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"15{Br}", output.ToString());
	}
	#endregion

	#region Equality Operators
	[TestMethod]
	public void Equality_ReturnsTrue_ForSameValues()
	{
		string source = """
print("hello" == "hello");
print(2.5 == 2.5);
print(true == true);
""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"True{Br}True{Br}True{Br}", output.ToString());
	}

	[TestMethod]
	public void Equality_ReturnsFalse_ForDifferentValues()
	{
		string source = """
print("hi" == "hello");
print(2.5 == -6);
print(true == false);
""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"False{Br}False{Br}False{Br}", output.ToString());
	}

	[TestMethod]
	public void Inequality_ReturnsTrue_ForDifferentValues()
	{
		string source = """
print("hi" != "hello");
print(2.5 != -6);
print(true != false);
""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"True{Br}True{Br}True{Br}", output.ToString());
	}

	[TestMethod]
	public void Inequality_ReturnsFalse_ForSameValues()
	{
		string source = """
print("hello" != "hello");
print(2.5 != 2.5);
print(true != true);
""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"False{Br}False{Br}False{Br}", output.ToString());
	}
	#endregion

	#region Comparison Operators
	[TestMethod]
	public void Comparison_GreaterThan_ComparesNumbersProperly()
	{
		string source = """
print(30 > 20);
print(20 > 20);
print(10 > 20);
""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"True{Br}False{Br}False{Br}", output.ToString());
	}

	[TestMethod]
	public void Comparison_GreaterThanOrEqual_ComparesNumbersProperly()
	{
		string source = """
print(30 >= 20);
print(20 >= 20);
print(10 >= 20);
""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"True{Br}True{Br}False{Br}", output.ToString());
	}

	[TestMethod]
	public void Comparison_LessThan_ComparesNumbersProperly()
	{
		string source = """
print(30 < 20);
print(20 < 20);
print(10 < 20);
""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"False{Br}False{Br}True{Br}", output.ToString());
	}

	[TestMethod]
	public void Comparison_LessThanOrEqual_ComparesNumbersProperly()
	{
		string source = """
print(30 <= 20);
print(20 <= 20);
print(10 <= 20);
""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"False{Br}True{Br}True{Br}", output.ToString());
	}
	#endregion

	#region Parentheses
	[TestMethod]
	public void Parentheses_CanBeAppliedToExpressions()
	{
		string source = """
print((4));
print((6+7));
print((((((("hello")))))));
""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"4{Br}13{Br}hello{Br}", output.ToString());
	}

	[TestMethod]
	public void Parentheses_AreExecutedFirst()
	{
		string source = "print(5 * (6 + 2));";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"40{Br}", output.ToString());
	}

	[TestMethod]
	public void Parentheses_WorkWithUnaryExpressions()
	{
		string source = "print(-(5+10));";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"-15{Br}", output.ToString());
	}
	#endregion
}