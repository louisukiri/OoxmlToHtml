using NUnit.Framework;

namespace OoxmlToHtml.test
{
    [TestFixture]
    public class OoxmlScannerTest
    {
        [Test]
        public void ShouldPeakAtNextTokenWithoutMovingTokenForward()
        {
            var scanner = new OoxmlScanner(new Source(@"<w:body>
                <w:t>text</w:t>
</w:body>"));

            scanner.NextToken();
            scanner.NextToken();
            scanner.NextToken();
            scanner.NextToken();
            scanner.NextToken();
            scanner.NextToken();
            var peek = scanner.PeekToken();
            var tokenAfterPeek = scanner.NextToken();
            var peek2 = scanner.PeekToken();

            Assert.AreEqual(KeywordToken.StringLiteral, peek.Keyword);
            Assert.AreEqual(KeywordToken.StringLiteral, tokenAfterPeek.Keyword);
            Assert.AreEqual(KeywordToken.Close, peek2.Keyword);
        }
    }
}