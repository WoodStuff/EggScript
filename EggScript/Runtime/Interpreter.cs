﻿using EggScript.Exceptions;
using EggScript.Parsing;
using EggScript.Parsing.Nodes.Expression;
using EggScript.Parsing.Nodes.Expression.Data;
using EggScript.Parsing.Nodes.Statement;

namespace EggScript.Runtime;

/// <summary>
/// Executes the EggScript program.
/// </summary>
internal static class Interpreter
{
	/// <summary>
	/// The declared variables and their values.
	/// </summary>
	private static Dictionary<string, Variable> Variables { get; } = [];

	/// <summary>
	/// Runs an EggScript program based on the abstract syntax tree generated from the <see cref="Parser"/>.
	/// </summary>
	/// <param name="nodes">The list of nodes generated by the <see cref="Parser"/>.</param>
	/// <exception cref="EggRuntimeException">Thrown when an invalid node is detected.</exception>
	public static void Run(List<IStatementNode> nodes)
	{
		foreach (IStatementNode node in nodes)
		{
			switch (node)
			{
				case PrintNode printNode:
					Console.WriteLine(GetValue(printNode.Data).Value);
					break;

				case VarDeclarationNode varDecNode:
					AddVariable(varDecNode.Name, GetValue(varDecNode.Data), varDecNode.Constant);
					break;

				case VarAssignmentNode varAssNode:
					ModifyVariable(varAssNode.Name, GetValue(varAssNode.Data));
					break;

				default:
					throw new EggRuntimeException($"Invalid node: {node.GetType().Name}");
			}
		}
	}

	/// <summary>
	/// Gets the value of an expression node.
	/// </summary>
	/// <param name="node">The expression node to evaluate.</param>
	/// <returns>The value of the expression.</returns>
	/// <exception cref="EggRuntimeException">Thrown when an invalid node is detected.</exception>
	private static IDataNode GetValue(IExpressionNode node)
	{
		return node switch
		{
			IDataNode dataNode => dataNode,
			VariableNode variableNode => GetVariable(variableNode.Name),
			OperatorNode operatorNode => ParseOperator(operatorNode),
			UnaryOpNode unaryOpNode => ParseOperator(unaryOpNode),
			_ => throw new EggRuntimeException($"Tried to get value of an invalid node: {node.GetType().Name}"),
		};
	}

	/// <summary>
	/// Calculates the result of an <see cref="OperatorNode"/>.
	/// </summary>
	/// <param name="node">The operator node to evaluate.</param>
	/// <returns>The result of the operator.</returns>
	/// <exception cref="EggRuntimeException">Thrown when an invalid use of operators is detected, for example wrong data types.</exception>
	private static IDataNode ParseOperator(OperatorNode node)
	{
		string op = node.Operator;
		IDataNode left = GetValue(node.Left);
		IDataNode right = GetValue(node.Right);

		return op switch
		{
			"+" => (left, right) switch
			{
				(NumberNode l, NumberNode r) => l + r,
				(StringNode l, StringNode r) => l + r,
				_ => throw new EggRuntimeException("Invalid data types in operator"),
			},
			"-" => (left, right) switch
			{
				(NumberNode l, NumberNode r) => l - r,
				_ => throw new EggRuntimeException("Invalid data types in operator"),
			},
			"*" => (left, right) switch
			{
				(NumberNode l, NumberNode r) => l * r,
				_ => throw new EggRuntimeException("Invalid data types in operator"),
			},
			"/" => (left, right) switch
			{
				(NumberNode l, NumberNode r) => l / r,
				_ => throw new EggRuntimeException("Invalid data types in operator"),
			},
			"&" => (left, right) switch
			{
				(BooleanNode l, BooleanNode r) => l & r,
				_ => throw new EggRuntimeException("Invalid data types in operator"),
			},
			"|" => (left, right) switch
			{
				(BooleanNode l, BooleanNode r) => l | r,
				_ => throw new EggRuntimeException("Invalid data types in operator"),
			},
			"==" => (left, right) switch
			{
				(StringNode l, StringNode r) => new BooleanNode(l.Value == r.Value),
				(NumberNode l, NumberNode r) => new BooleanNode(l.Value == r.Value),
				(BooleanNode l, BooleanNode r) => new BooleanNode(l.Value == r.Value),
				_ => throw new EggRuntimeException("Invalid data types in operator"),
			},
			"!=" => (left, right) switch
			{
				(StringNode l, StringNode r) => new BooleanNode(l.Value != r.Value),
				(NumberNode l, NumberNode r) => new BooleanNode(l.Value != r.Value),
				(BooleanNode l, BooleanNode r) => new BooleanNode(l.Value != r.Value),
				_ => throw new EggRuntimeException("Invalid data types in operator"),
			},
			">" => (left, right) switch
			{
				(NumberNode l, NumberNode r) => l > r,
				_ => throw new EggRuntimeException("Invalid data types in operator"),
			},
			"<" => (left, right) switch
			{
				(NumberNode l, NumberNode r) => l < r,
				_ => throw new EggRuntimeException("Invalid data types in operator"),
			},
			">=" => (left, right) switch
			{
				(NumberNode l, NumberNode r) => l >= r,
				_ => throw new EggRuntimeException("Invalid data types in operator"),
			},
			"<=" => (left, right) switch
			{
				(NumberNode l, NumberNode r) => l <= r,
				_ => throw new EggRuntimeException("Invalid data types in operator"),
			},
			"=" => node.Left switch
			{
				VariableNode l => ModifyVariable(l.Name, right),
				_ => throw new EggRuntimeException("Invalid data types in operator"),
			},
			_ => throw new EggRuntimeException("Invalid operator"),
		};
	}

	/// <summary>
	/// Calculates the result of an <see cref="UnaryOpNode"/>.
	/// </summary>
	/// <param name="node">The operator node to evaluate.</param>
	/// <returns>The result of the operator.</returns>
	/// <exception cref="EggRuntimeException">Thrown when an invalid use of operators is detected, for example wrong data types.</exception>
	private static IDataNode ParseOperator(UnaryOpNode node)
	{
		string op = node.Operator;
		IDataNode operand = GetValue(node.Operand);

		return op switch
		{
			"+" => operand switch
			{
				NumberNode n => +n,
				_ => throw new EggRuntimeException("Invalid data types in operator"),
			},
			"-" => operand switch
			{
				NumberNode n => -n,
				_ => throw new EggRuntimeException("Invalid data types in operator"),
			},
			"!" => operand switch
			{
				BooleanNode n => !n,
				_ => throw new EggRuntimeException("Invalid data types in operator"),
			},
			_ => throw new EggRuntimeException("Invalid operator"),
		};
	}

	private static void AddVariable(string name, IDataNode data, bool constant = false)
	{
		Variable var = new(data, constant);
		if (!Variables.TryAdd(name, var)) throw new EggRuntimeException($"Variable {name} was already declared");
	}

	private static IDataNode GetVariable(string name)
	{
		if (!Variables.TryGetValue(name, out Variable? var)) throw new EggRuntimeException($"Variable {name} was not found");
		return var.Data;
	}
	
	private static IDataNode ModifyVariable(string name, IDataNode data)
	{
		if (!Variables.TryGetValue(name, out Variable? value)) throw new EggRuntimeException($"Variable {name} was not found");
		if (value.Constant) throw new EggRuntimeException($"Cannot modify constant variable {name}");
		if (value.Type != data.Type) throw new EggRuntimeException($"Cannot change variable {name}'s type (tried to change {value.Type} to {value.Type})");
		Variables[name].Data = data;
		return data;
	}
}