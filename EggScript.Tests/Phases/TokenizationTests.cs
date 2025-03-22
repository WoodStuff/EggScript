using EggScript.Tokenization;

namespace EggScript.Tests.Phases;

[TestClass]
public sealed class TokenizationTests
{
	[TestMethod]
	public void Tokenizer_AssignsCorrectTokens()
	{
		string source = "print(2.5 > 7 & true);";
		var tokens = Tokenizer.Tokenize(source);
		Assert.AreEqual(new(TokenType.Keyword, "print"), tokens[0]);
		Assert.AreEqual(new(TokenType.Punctuation, "("), tokens[1]);
		Assert.AreEqual(new(TokenType.Number, "2.5"), tokens[2]);
		Assert.AreEqual(new(TokenType.Operator, ">"), tokens[3]);
		Assert.AreEqual(new(TokenType.Number, "7"), tokens[4]);
		Assert.AreEqual(new(TokenType.Operator, "&"), tokens[5]);
		Assert.AreEqual(new(TokenType.FreeKeyword, "true"), tokens[6]);
		Assert.AreEqual(new(TokenType.Punctuation, ")"), tokens[7]);
		Assert.AreEqual(new(TokenType.Punctuation, ";"), tokens[8]);
	}

	[TestMethod]
	public void Tokenizer_MarksEndOfFile()
	{
		string source = "2";
		var tokens = Tokenizer.Tokenize(source);
		Assert.AreEqual(TokenType.EOF, tokens[^1].Type);
	}
}
