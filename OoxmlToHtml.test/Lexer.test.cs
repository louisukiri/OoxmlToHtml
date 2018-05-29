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
w:color w:val = /> </w:p w:pPr w:b w:i w:sz xml:space w:pStyle";
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
                new Tokens(Tokens.SpaceAttribute, "xml:space"),
                new Tokens(Tokens.ParagraphStyle, "w:pStyle"),
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


        [Test]
        public void TestTextTokens()
        {
            var input = @"<w:t>test```another one```some more text</w:t>";
            Tokens[] expected = new Tokens[]
            {
                new Tokens(Tokens.Start.ToString(), "<"),
                new Tokens(Tokens.Text, "w:t"),
                new Tokens(Tokens.End.ToString(), ">"),
                new Tokens(Tokens.StringLiteral, "test"), 
                new Tokens(Tokens.Code, "```"),
                new Tokens(Tokens.StringLiteral, "another"),
                new Tokens(Tokens.StringLiteral, "one"),
                new Tokens(Tokens.Code, "```"),
                new Tokens(Tokens.StringLiteral, "some"),
                new Tokens(Tokens.StringLiteral, "more"),
                new Tokens(Tokens.StringLiteral, "text"),
                new Tokens(Tokens.LongEnd, "w:t"),
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
