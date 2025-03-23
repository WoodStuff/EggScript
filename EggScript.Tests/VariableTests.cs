using EggScript.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EggScript.Tests;

[TestClass]
public sealed class VariableTests
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

	#region Variable declarations
	[TestMethod]
	public void Declaration_DeclaresString()
	{
		string source = """
string text = "hello";
print(text);
""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"hello{Br}", output.ToString());
	}

	[TestMethod]
	public void Declaration_DeclaresNumber()
	{
		string source = """
num seven = 7;
print(seven);
""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"7{Br}", output.ToString());
	}

	[TestMethod]
	public void Declaration_DeclaresBoolean()
	{
		string source = """
bool flag = true;
print(flag);
""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"True{Br}", output.ToString());
	}

	[TestMethod]
	public void Declaration_WhenUsedUninitialized_ThrowsError()
	{
		string source = """
string empty;
print(empty);
""";
		Assert.ThrowsException<EggRuntimeException>(() => Eggscript.ExecuteDirect(source));
	}
	#endregion

	#region Variable assignments
	[TestMethod]
	public void Assignment_ModifiesVariable()
	{
		string source = """
string text = "hello";
print(text);
text = "line two";
print(text);
""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"hello{Br}line two{Br}", output.ToString());
	}

	[TestMethod]
	public void Assignment_InitializesUninitializedVariable()
	{
		string source = """
string text;
text = "hello";
print(text);
""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"hello{Br}", output.ToString());
	}
	#endregion

	#region Expressions with variables
	[TestMethod]
	public void Declaration_SupportsExpressions()
	{
		string source = """
num result = 2 + 7 * 8;
print(result);
""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"58{Br}", output.ToString());
	}

	[TestMethod]
	public void Assignment_SupportsExpressions()
	{
		string source = """
num result;
result = 2 + 7 * 8;
print(result);
""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"58{Br}", output.ToString());
	}

	[TestMethod]
	public void Variable_CanBeSetToOtherVariable()
	{
		string source = """
num three = 3;
num result = three;
print(result);
""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"3{Br}", output.ToString());
	}

	[TestMethod]
	public void Variable_CanBeModifiedWithOperators()
	{
		string source = """
num five = 5;
print(five + 6);
""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"11{Br}", output.ToString());
	}
	#endregion

	#region Const variables
	[TestMethod]
	public void ConstVariable_HoldsData()
	{
		string source = """
const string text = "hello";
const num seven = 7;
const bool flag = true;
print(text);
print(seven);
print(flag);
""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"hello{Br}7{Br}True{Br}", output.ToString());
	}

	[TestMethod]
	public void ConstVariable_WhenReassigned_ThrowsError()
	{
		string source = """
const string text = "hello";
text = "line two";
""";
		Assert.ThrowsException<EggRuntimeException>(() => Eggscript.ExecuteDirect(source));
	}

	[TestMethod]
	public void ConstVariable_WhenDeclaredUninitialized_ThrowsError()
	{
		string source = """
const num something;
""";
		Assert.ThrowsException<EggSyntaxException>(() => Eggscript.ExecuteDirect(source));
	}
	#endregion
}