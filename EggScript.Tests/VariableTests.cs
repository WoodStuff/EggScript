using EggScript.Exceptions;

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
var text [string] = "hello";
print(text);
""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"hello{Br}", output.ToString());
	}

	[TestMethod]
	public void Declaration_DeclaresNumber()
	{
		string source = """
var seven [num] = 7;
print(seven);
""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"7{Br}", output.ToString());
	}

	[TestMethod]
	public void Declaration_DeclaresBoolean()
	{
		string source = """
var flag [bool] = true;
print(flag);
""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"true{Br}", output.ToString());
	}

	[TestMethod]
	public void Declaration_WhenUsedUninitialized_ThrowsError()
	{
		string source = """
var empty [string];
print(empty);
""";
		Assert.ThrowsException<EggRuntimeException>(() => Eggscript.ExecuteDirect(source));
	}

	[TestMethod]
	public void Declaration_WhenUsedAsExpression_ThrowsError()
	{
		string source = """
print(var text [string] = "test");
print(text);
""";
		Assert.ThrowsException<EggSyntaxException>(() => Eggscript.ExecuteDirect(source));

	}
	#endregion

	#region Variable assignments
	[TestMethod]
	public void Assignment_ModifiesVariable()
	{
		string source = """
var text [string] = "hello";
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
var text [string];
text = "hello";
print(text);
""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"hello{Br}", output.ToString());
	}

	[TestMethod]
	public void Assignment_CanBeUsedAsExpression()
	{
		string source = """
var text [string];
print(text = "test");
print(text);
""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"test{Br}test{Br}", output.ToString());
	}
	#endregion

	#region Expressions with variables
	[TestMethod]
	public void Declaration_SupportsExpressions()
	{
		string source = """
var result [num] = 2 + 7 * 8;
print(result);
""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"58{Br}", output.ToString());
	}

	[TestMethod]
	public void Assignment_SupportsExpressions()
	{
		string source = """
var result [num];
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
var three [num] = 3;
var result [num] = three;
print(result);
""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"3{Br}", output.ToString());
	}

	[TestMethod]
	public void Variable_CanBeModifiedWithOperators()
	{
		string source = """
var five [num] = 5;
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
const text [string] = "hello";
const seven [num] = 7;
const flag [bool] = true;
print(text);
print(seven);
print(flag);
""";
		Eggscript.ExecuteDirect(source);

		Assert.AreEqual($"hello{Br}7{Br}true{Br}", output.ToString());
	}

	[TestMethod]
	public void ConstVariable_WhenReassigned_ThrowsError()
	{
		string source = """
const text [string] = "hello";
text = "line two";
""";
		Assert.ThrowsException<EggRuntimeException>(() => Eggscript.ExecuteDirect(source));
	}

	[TestMethod]
	public void ConstVariable_WhenDeclaredUninitialized_ThrowsError()
	{
		string source = """
const something [num];
""";
		Assert.ThrowsException<EggSyntaxException>(() => Eggscript.ExecuteDirect(source));
	}
	#endregion
}