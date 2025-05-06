using System.Diagnostics.CodeAnalysis;
using EggScript.Exceptions;
using EggScript.Parsing.Nodes.Expression.Data;

namespace EggScript.Parsing;

/// <summary>
/// An EggScript variable.
/// </summary>
/// <param name="data">The data the variable stores.</param>
internal class Variable(DataType type, bool constant = false)
{
	private IDataNode? _data = null;
	/// <summary>
	/// The data the variable stores.
	/// </summary>
	public IDataNode? Data
	{
		get => _data;
		set
		{
			if (value is null) throw new EggRuntimeException("Invalid value");
			if (value.Type != Type) throw new EggRuntimeException($"Cannot change a variable's type (tried to change {Type} to {value.Type})");
			_data = value;
		}
	}
	/// <summary>
	/// The variable's type. This cannot be changed.
	/// </summary>
	public DataType Type { get; } = type;
	/// <summary>
	/// If a variable is constant, its value cannot be reassigned.
	/// </summary>
	public bool Constant { get; } = constant;

	/// <summary>
	/// If the variable is initialized. An uninitialized variable is defined like: var txt [string];
	/// </summary>
	[MemberNotNullWhen(true, nameof(Data))]
	public bool Initialized => Data != null;
}