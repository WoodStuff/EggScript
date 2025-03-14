using EggScript.Exceptions;
using EggScript.Parsing.Nodes.Expression.Data;

namespace EggScript.Parsing;

/// <summary>
/// An EggScript variable.
/// </summary>
/// <param name="data">The data the variable stores.</param>
internal class Variable(IDataNode data)
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
			if (value.Type != Type) throw new EggRuntimeException($"Cannot change a variable's type");
			_data = value;
		}
	}

	/// <summary>
	/// The variable's type. This cannot be changed.
	/// </summary>
	public DataType Type => Data.Type;
}