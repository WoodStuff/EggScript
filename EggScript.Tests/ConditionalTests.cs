using EggScript.Exceptions;

namespace EggScript.Tests;

[TestClass]
public sealed class ConditionalTests
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

	#region Branch running
	[TestMethod]
	public void If_RunsMainBody_IfConditionIsTrue()
	{
		string source = """if true { print("text"); }""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"text{Br}", output.ToString());
	}

	[TestMethod]
	public void If_DoesNotRunMainBody_IfConditionIsFalse()
	{
		string source = """if false { print("text"); }""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"", output.ToString());
	}

	[TestMethod]
	public void IfElse_RunsMainBody_IfConditionIsTrue()
	{
		string source = """if true { print(1); } else { print(2); }""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"1{Br}", output.ToString());
	}

	[TestMethod]
	public void IfElse_RunsElseBody_IfConditionIsFalse()
	{
		string source = """if false { print(1); } else { print(2); }""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"2{Br}", output.ToString());
	}
	#endregion

	#region Body types
	[TestMethod]
	public void Conditional_SupportsMultipleStatementBody()
	{
		string source = """
			if true { print(1); print(2); } else { print(3); print(4); }
			if false { print(5); print(6); } else { print(7); print(8); }
			""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"1{Br}2{Br}7{Br}8{Br}", output.ToString());
	}

	[TestMethod]
	public void Conditional_SupportsUnbracedBody()
	{
		string source = """
			if true print(1); else print(2);
			if false print(3); else print(4);
			""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"1{Br}4{Br}", output.ToString());
	}

	[TestMethod]
	public void Conditional_SupportsEmptyBody()
	{
		string source = """if true { } else { }""";
		Eggscript.ExecuteDirect(source);
	}

	[TestMethod]
	public void Conditional_SupportsElseIf()
	{
		string source = """
			if true { print(1); } else if true { print(2); } else { print(3); }
			if false { print(4); } else if true { print(5); } else { print(6); }
			if false { print(7); } else if false { print(8); } else { print(9); }
			""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"1{Br}5{Br}9{Br}", output.ToString());
	}
	#endregion

	#region Conditions
	[TestMethod]
	public void Conditional_SupportsAdvancedConditions()
	{
		string source = """
			if 4 > 2 { print("main 1"); } else { print("else 1"); }
			if 2 > 4 { print("main 2"); } else { print("else 2"); }
			""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"main 1{Br}else 2{Br}", output.ToString());
	}

	[TestMethod]
	public void Conditional_SupportsVariableConditions()
	{
		string source = """
			var X [num] = 4;
			if X > 2 { print("main 1"); } else { print("else 1"); }
			if 2 > X { print("main 2"); } else { print("else 2"); }
			""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"main 1{Br}else 2{Br}", output.ToString());
	}

	[TestMethod]
	public void Conditional_SupportsConditionInParentheses()
	{
		string source = """
			if (true) { print(1); }
			if (5 == 5) { print(2); }
			""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"1{Br}2{Br}", output.ToString());
	}
	#endregion
}