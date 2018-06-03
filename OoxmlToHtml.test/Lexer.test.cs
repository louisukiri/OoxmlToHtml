using System.Collections.Generic;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace OoxmlToHtml.test
{
    [TestFixture]
    public class LexerTest
    {
        OoxmlScanner l = new OoxmlScanner(new Source(@"<>
w:p w:r w:t ""testvalue"" someOtherText
w:color w:val = /> </w:p w:pPr w:b w:i w:sz xml:space w:pStyle"));
        [SetUp]
        public void Setup()
        {

        }
        [Test]
        public void TestNextToken()
        {
            var expected = new Dictionary<KeywordToken, string>
            {
                { KeywordToken.STARTING_ELEMENT, "<" },
                { KeywordToken.ENDING_ELEMENT, ">" },
                { KeywordToken.Paragraph, "w:p" },
                { KeywordToken.Run, "w:r" },
                { KeywordToken.Text, "w:t" },
                { KeywordToken.StringValue, "testvalue" },
                { KeywordToken.StringLiteral, "someOtherText" },
                { KeywordToken.Color, "w:color" },
                { KeywordToken.Value, "w:val" },
                { KeywordToken.EQ, "=" },
                { KeywordToken.ShortClose, "/>" },
                { KeywordToken.Close, "w:p" },
                { KeywordToken.PreviousParagraph, "w:pPr" },
                { KeywordToken.Bold, "w:b" },
                { KeywordToken.Italic, "w:i" },
                { KeywordToken.Size, "w:sz" },
                { KeywordToken.Space, "xml:space" },
                { KeywordToken.ParagraphStyle, "w:pStyle" },
                { KeywordToken.EOF, null }
            };

            foreach(var i in expected.Keys)
            {
                var expectedToken = i;
                var expectedText = expected[i];
                var token = l.NextToken();
                if (token.Keyword != expectedToken)
                {
                    Assert.Fail("Token type. Expected {0} got {1}", expectedToken, token.Keyword);
                }

                if (expectedText != token.Text)
                {
                    Assert.Fail("Token Literal {0} got {1}", expectedText, token.Text);
                }
            }
        }


        [Test]
        public void TestTextTokens()
        {
            var input = @"<w:t>test```another one```some more text</w:t>";
            Token[] expected = new Token[]
            {
                new Token(Token.Start.ToString(), "<"),
                new Token(Token.End.ToString(), ">"),
                new Token(Token.StringLiteral, "test"), 
                new Token(Token.Code, "```"),
                new Token(Token.StringLiteral, "another"),
                new Token(Token.StringLiteral, "one"),
                new Token(Token.Code, "```"),
                new Token(Token.StringLiteral, "some"),
                new Token(Token.StringLiteral, "more"),
                new Token(Token.StringLiteral, "text"),
                new Token(Token.LongEnd, "w:t"),
                new Token(Token.Eof, "EOF")
            };

            var l = new Lexer(input);

            foreach (var expectedToken in expected)
            {
                var token = l.NextToken();
                if (token.Type != expectedToken.Type)
                {
                    Assert.Fail("Token type. Expected {0} got {1}", expectedToken.Type, token.Type);
                }

                if (token.Literal != expectedToken.Literal)
                {
                    Assert.Fail("Token Literal {0} got {1}", expectedToken.Literal, token.Literal);
                }
            }
        }
    }
}
