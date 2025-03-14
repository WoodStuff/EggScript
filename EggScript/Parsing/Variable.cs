using EggScript.Exceptions;
using EggScript.Parsing.Nodes.Expression.Data;

namespace EggScript.Parsing;

/// <summary>
/// An EggScript variable.
/// </summary>
/// <param name="data">The data the variable stores.</param>
internal class Variable(IDataNode data, bool constant = false)
{
	private IDataNode _data = data;
	/// <summary>
	/// The data the variable stores.
	/// </summary>
	public IDataNode Data
	{
		get => _data;
		set
		{
			if (Constant) throw new EggRuntimeException("Cannot modify a constant variable");
			if (value.Type != Type) throw new EggRuntimeException($"Cannot change a variable's type (tried to change {Type} to {value.Type})");
			_data = value;
		}
	}
	/// <summary>
	/// If a variable is constant, its value cannot be reassigned.
	/// </summary>
	public bool Constant { get; } = constant;

	/// <summary>
	/// The variable's type. This cannot be changed.
	/// </summary>
	public DataType Type => _data.Type;
}