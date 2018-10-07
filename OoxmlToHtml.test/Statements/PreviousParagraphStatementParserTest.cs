using NUnit.Framework;
using OoxmlToHtml.Parsers;

namespace OoxmlToHtml.test.Statements
{
    [TestFixture]
    public class PreviousParagraphStatementParserTest
    {
        [Test]
        public void ParseShouldLeaveCursorOneTokenPastTheParagraphBoundary()
        {
            var parserNode = new OoxmlNodeTd(new OoxmlScanner(
                new Source(@"<w:body>
                                <w:p>
                                    <w:pPr>
                                        <w:i />
                                    </w:pPr>
                                </w:p>
                            </w:body>")
            ));
            parserNode.NextToken();
            while (parserNode.CurrentToken.Keyword != KeywordToken.PreviousParagraph)
            {
                parserNode.NextToken();
            }
            var sut = new PreviousParagraphStatementParser(parserNode);
            sut.Parse(parserNode.CurrentToken);

            var currentToken = parserNode.CurrentToken;
            Assert.AreEqual(KeywordToken.Close, currentToken.Keyword);
            Assert.AreEqual("w:p", currentToken.Text);
        }

        [Test]
        public void ParseShouldSkipUnrecognizedToken()
        {
            var parserNode = new OoxmlNodeTd(new OoxmlScanner(
                new Source(@"<w:body>
                                <w:p>
                                    <w:pPr>
                                        <w:rPr>
                                            <w:i />
                                        </w:rPr>
                                    </w:pPr>
                                </w:p>
                            </w:body>")
            ));
            parserNode.NextToken();
            while (parserNode.CurrentToken.Keyword != KeywordToken.PreviousParagraph)
            {
                parserNode.NextToken();
            }
            var sut = new PreviousParagraphStatementParser(parserNode);
            sut.Parse(parserNode.CurrentToken);

            var currentToken = parserNode.CurrentToken;
            Assert.AreEqual(KeywordToken.Close, currentToken.Keyword);
            Assert.AreEqual("w:p", currentToken.Text);
        }
    }
}