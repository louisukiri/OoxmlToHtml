using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace OoxmlToHtml.test
{
    [TestFixture]
    public class LexerTest
    {
        [SetUp]
        public void Setup()
        {

        }
        [Test]
        public void TestNextToken()
        {
            var l = new OoxmlScanner(new Source(@"<>
w:p w:r w:t ""testvalue"" someOtherText
w:color w:val = /> </w:p> w:pPr w:b w:i w:sz xml:space w:pStyle"));
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

            foreach (var i in expected.Keys)
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
        public void TestDuplicateTokens()
        {
            var l = new OoxmlScanner(new Source(@"w:rPr"));
            var expected = new Dictionary<KeywordToken, string>
            {
                { KeywordToken.PreviousParagraph, "w:rPr" },
                { KeywordToken.EOF, null }
            };

            foreach (var i in expected.Keys)
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
        public void TestCodeToken()
        {
            OoxmlScanner l = new OoxmlScanner(new Source(@"<w:t>test```another one```some more text</w:t>"));

            var expected2 = new Tuple<KeywordToken, string>[]
            {
                new Tuple<KeywordToken, string>(KeywordToken.STARTING_ELEMENT, "<"),
                new Tuple<KeywordToken, string>(KeywordToken.Text, "w:t"),
                new Tuple<KeywordToken, string>(KeywordToken.ENDING_ELEMENT, ">"),
                new Tuple<KeywordToken, string>(KeywordToken.StringLiteral, "test"),
                new Tuple<KeywordToken, string>(KeywordToken.Code, ""),
                new Tuple<KeywordToken, string>(KeywordToken.StringLiteral, "another"),
                new Tuple<KeywordToken, string>(KeywordToken.StringLiteral, "one"),
                new Tuple<KeywordToken, string>(KeywordToken.Code, ""),
                new Tuple<KeywordToken, string>(KeywordToken.StringLiteral, "some"),
                new Tuple<KeywordToken, string>(KeywordToken.StringLiteral, "more"),
                new Tuple<KeywordToken, string>(KeywordToken.StringLiteral, "text"),
                new Tuple<KeywordToken, string>(KeywordToken.Close, "w:t")
            };
            foreach (var tuple in expected2)
            {
                var expectedToken = tuple.Item1;
                var expectedText = tuple.Item2;
                var token = l.NextToken();

                if (token.Keyword != expectedToken)
                {
                    Assert.Fail("Token type. Expected {0} got {1}", expectedToken, token.Keyword);
                }

                if (token.Text != expectedText)
                {
                    Assert.Fail("Token Literal {0} got {1}", expectedText, token.Text);
                }
            }
        }
    }
}
