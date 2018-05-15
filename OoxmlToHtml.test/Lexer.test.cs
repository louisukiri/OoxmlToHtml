using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace OoxmlToHtml.test
{
    [TestFixture]
    public class LexerTest
    {
        [Test]
        public void TestNextToken()
        {
            var input = @"<>
w:p w:r w:t "" someOtherText
w:color w:val = /> </w:p w:pPr w:b w:i w:sz";
            Tokens [] expected = new Tokens[]
            {
                new Tokens(Tokens.Start.ToString(), "<"),
                new Tokens(Tokens.End.ToString(), ">"),
                new Tokens(Tokens.Paragraph, "w:p"),
                new Tokens(Tokens.Run, "w:r"),
                new Tokens(Tokens.Text, "w:t"),
                new Tokens(Tokens.Quote.ToString(), "\""),
                new Tokens(Tokens.StringLiteral, "someOtherText"),
                new Tokens(Tokens.Color, "w:color"),
                new Tokens(Tokens.Value, "w:val"),
                new Tokens(Tokens.EQ.ToString(), "="),
                new Tokens(Tokens.ShortEnd, "/>"),
                new Tokens(Tokens.LongEnd, "w:p"),
                new Tokens(Tokens.PreviousParagraph, "w:pPr"),
                new Tokens(Tokens.Bold, "w:b"),
                new Tokens(Tokens.Italic, "w:i"),
                new Tokens(Tokens.Size, "w:sz"), 
                new Tokens(Tokens.Eof, "EOF")
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
