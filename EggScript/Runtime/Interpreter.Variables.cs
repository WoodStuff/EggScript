using EggScript.Exceptions;
using EggScript.Parsing;
using EggScript.Parsing.Nodes.Expression.Data;

namespace EggScript.Runtime;

/// <summary>
/// Executes the EggScript program.
/// </summary>
internal static partial class Interpreter
{
	private static Stack<Scope> Scopes { get; } = [];
	private static Scope CurrentScope => Scopes.Peek();

	/// <summary>
	/// Declares a variable with the given name and data.
	/// </summary>
	/// <param name="name">The name of the variable.</param>
	/// <param name="data">The variable's value.</param>
	/// <param name="constant">If the variable is a constant.</param>
	/// <exception cref="EggRuntimeException">Thrown when the variable has already been declared.</exception>
	private static void AddVariable(string name, DataType type, bool constant = false) => CurrentScope.AddVariable(name, type, constant);

	/// <summary>
	/// Gets a variable's value.
	/// </summary>
	/// <param name="name">The name of the variable.</param>
	/// <returns>The variable's value.</returns>
	/// <exception cref="EggRuntimeException">Thrown when the variable has not been declared yet.</exception>
	private static IDataNode GetVariable(string name)
	{
		foreach (var scope in Scopes)
		{
			if (scope.TryGetVariable(name, out IDataNode? data)) return data;
		}
		throw new EggRuntimeException($"Variable {name} was not declared in this scope");
	}

	/// <summary>
	/// Assigns a value to a declared variable.
	/// </summary>
	/// <param name="name">The name of the variable.</param>
	/// <param name="data">The value to set the variable to.</param>
	/// <param name="init">If this is a variable initialization (num n = 2) as opposed to an assignment (n = 2).</param>
	/// <returns><paramref name="data"/></returns>
	/// <exception cref="EggRuntimeException">Thrown when the variable has not been declared yet, is a constant variable, or an attempt to change the variable's data type was detected.</exception>
	private static IDataNode ModifyVariable(string name, IDataNode data, bool init = false)
	{
		foreach (var scope in Scopes)
		{
			if (!scope.ContainsVariable(name)) continue;
			scope.ModifyVariable(name, data, init);
			return data;
		}
		throw new EggRuntimeException($"Variable {name} was not declared in this scope");
	}
}