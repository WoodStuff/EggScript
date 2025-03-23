using EggScript.Exceptions;

namespace EggScript.Tests;

[TestClass]
public sealed class BaseTests
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

	#region Print - Primitive expressions
	[TestMethod]
	public void PrintKeyword_PrintsString()
	{
		string source = """print("hello");""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"hello{Br}", output.ToString());
	}

	[TestMethod]
	public void PrintKeyword_PrintsNumber()
	{
		string source = """print(-7.3);""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"-7.3{Br}", output.ToString());
	}

	[TestMethod]
	public void PrintKeyword_PrintsBoolean()
	{
		string source = """print(true);""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"true{Br}", output.ToString());
	}
	#endregion

	#region Print - Errors
	[TestMethod]
	public void PrintKeyword_WithoutArguments_ThrowsError()
	{
		string source = """print();""";
		Assert.ThrowsException<EggSyntaxException>(() => Eggscript.ExecuteDirect(source));
	}

	[TestMethod]
	public void PrintKeyword_WithoutSemicolon_ThrowsError()
	{
		string source = """print("hello")""";
		Assert.ThrowsException<EggSyntaxException>(() => Eggscript.ExecuteDirect(source));
	}
	#endregion

	#region Program tests
	[TestMethod]
	public void Program_ExecutesWithNoStatements()
	{
		string source = "";
		Eggscript.ExecuteDirect(source);
	}

	[TestMethod]
	public void Program_ExecutesWithMultipleStatements()
	{
		string source = """
print("first line");
print("second line");
""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"first line{Br}second line{Br}", output.ToString());
	}
	#endregion
}