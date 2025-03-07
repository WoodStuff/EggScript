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
	/// Runs an EggScript program based on the abstract syntax tree generated from the <see cref="Parser"/>.
	/// </summary>
	/// <param name="nodes">The list of nodes generated by the <see cref="Parser"/>.</param>
	/// <exception cref="EggScriptException">Thrown when an invalid node is detected.</exception>
	public static void Run(List<IStatementNode> nodes)
	{
		foreach (IStatementNode node in nodes)
		{
			switch (node)
			{
				case PrintNode printNode:
					Console.WriteLine(GetValue(printNode.Data).Value);
					break;

				default:
					throw new EggScriptException("Invalid node");
			}
		}
	}

	/// <summary>
	/// Gets the value of an expression node.
	/// </summary>
	/// <param name="node">The expression node to evaluate.</param>
	/// <returns>The value of the expression.</returns>
	/// <exception cref="EggScriptException">Thrown when an invalid node is detected.</exception>
	private static IDataNode GetValue(IExpressionNode node)
	{
		return node switch
		{
			IDataNode dataNode => dataNode,
			OperatorNode operatorNode => ParseOperator(operatorNode),
			_ => throw new EggScriptException("Tried to get value of an invalid node"),
		};
	}

	/// <summary>
	/// Calculates the result of an <see cref="OperatorNode"/>.
	/// </summary>
	/// <param name="node">The operator node to evaluate.</param>
	/// <returns>The result of the operator.</returns>
	/// <exception cref="EggScriptException">Thrown when an invalid use of operators is detected, for example wrong data types.</exception>
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
				_ => throw new EggScriptException("Invalid data types in operator"),
			},
			"-" => (left, right) switch
			{
				(NumberNode l, NumberNode r) => l - r,
				_ => throw new EggScriptException("Invalid data types in operator"),
			},
			"*" => (left, right) switch
			{
				(NumberNode l, NumberNode r) => l * r,
				_ => throw new EggScriptException("Invalid data types in operator"),
			},
			"/" => (left, right) switch
			{
				(NumberNode l, NumberNode r) => l / r,
				_ => throw new EggScriptException("Invalid data types in operator"),
			},
			_ => throw new EggScriptException("Invalid operator"),
		};
	}
}