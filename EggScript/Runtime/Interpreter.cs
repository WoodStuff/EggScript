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
	private static Dictionary<string, IDataNode> Variables { get; } = [];

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

				case VarDeclarationNode varDeclarationNode:
					AddVariable(varDeclarationNode.Name, GetValue(varDeclarationNode.Data));
					break;

				case VarAssignmentNode varAssignmentNode:
					ModifyVariable(varAssignmentNode.Name, GetValue(varAssignmentNode.Data));
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
			VariableNode variableNode => GetVariable(variableNode.Value),
			IDataNode dataNode => dataNode,
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
				VariableNode l => ModifyVariable(l.Value, right),
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

	private static void AddVariable(string name, IDataNode data)
	{
		if (!Variables.TryAdd(name, data)) throw new EggRuntimeException($"Variable {name} was already declared");
	}

	private static IDataNode GetVariable(string name)
	{
		if (!Variables.TryGetValue(name, out IDataNode? var)) throw new EggRuntimeException($"Variable {name} was not found");
		return var;
	}
	
	private static IDataNode ModifyVariable(string name, IDataNode data)
	{
		if (!Variables.ContainsKey(name)) throw new EggRuntimeException($"Variable {name} was not found");
		Variables[name] = data;
		return data;
	}
}