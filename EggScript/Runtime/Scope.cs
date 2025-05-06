using System.Diagnostics.CodeAnalysis;
using EggScript.Exceptions;
using EggScript.Parsing;
using EggScript.Parsing.Nodes.Expression.Data;

namespace EggScript.Runtime;

/// <summary>
/// A scope, which holds variables.
/// </summary>
internal class Scope
{
	/// <summary>
	/// The declared variables and their values.
	/// </summary>
	private Dictionary<string, Variable> Variables { get; } = [];

	/// <summary>
	/// Checks if this scope contains a variable. If not, a previous scope might contain it.
	/// </summary>
	/// <param name="name">The name of the variable.</param>
	/// <returns>If this scope contains the variable.</returns>
	public bool ContainsVariable(string name) => Variables.ContainsKey(name);

	/// <summary>
	/// Declares a variable with the given name and data.
	/// </summary>
	/// <param name="name">The name of the variable.</param>
	/// <param name="type">The variable's type.</param>
	/// <param name="constant">If the variable is a constant.</param>
	/// <exception cref="EggRuntimeException">Thrown when the variable has already been declared.</exception>
	public void AddVariable(string name, DataType type, bool constant = false)
	{
		Variable var = new(type, constant);
		if (!Variables.TryAdd(name, var)) throw new EggRuntimeException($"Variable {name} was already declared in this scope");
	}

	/// <summary>
	/// Tries to get a variable's value.
	/// </summary>
	/// <param name="name">The name of the variable.</param>
	/// <param name="data">The data the variable holds, if the variable was found.</param>
	/// <returns>If the variable was successfully found.</returns>
	/// <exception cref="EggRuntimeException">Thrown when the variable has not been declared yet.</exception>
	public bool TryGetVariable(string name, [NotNullWhen(true)] out IDataNode? data)
	{
		bool success = Variables.TryGetValue(name, out Variable? var) && var.Initialized;
		data = var?.Data;
		return success;
	}

	/// <summary>
	/// Assigns a value to a declared variable.
	/// </summary>
	/// <param name="name">The name of the variable.</param>
	/// <param name="data">The value to set the variable to.</param>
	/// <param name="init">If this is a variable initialization (num n = 2) as opposed to an assignment (n = 2).</param>
	/// <returns><paramref name="data"/></returns>
	/// <exception cref="EggRuntimeException">Thrown when the variable has not been declared yet, is a constant variable, or an attempt to change the variable's data type was detected.</exception>
	public IDataNode ModifyVariable(string name, IDataNode data, bool init = false)
	{
		if (!Variables.TryGetValue(name, out Variable? value)) throw new EggRuntimeException($"Variable {name} was not declared in this scope");
		if (!init && value.Constant) throw new EggRuntimeException($"Cannot modify constant variable {name}");
		if (value.Type != data.Type) throw new EggRuntimeException($"Cannot change variable {name}'s type (tried to change {value.Type} to {data.Type})");
		Variables[name].Data = data;
		return data;
	}
}