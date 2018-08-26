using NUnit.Framework;
using OoxmlToHtml.test.Helpers;

namespace OoxmlToHtml.test.Statements
{
    [TestFixture]
    public class TextStatementParser
    {
        [Test]
        public void ShouldNotNestTextStatements()
        {
            var input = @"<w:body>
                    <w:p>
                      <w:pPr></w:pPr>
                        <w:r>                            
                            <w:t xml:space=""preserve"">Abc louis</w:t>
                        </w:r>
                    </w:p>
                    <w:p>
                      <w:pPr></w:pPr>
                        <w:r>
                            <w:t xml:space=""preserve"">22 louis</w:t>
                        </w:r>
                    </w:p>
                    <w:p>
                      <w:pPr></w:pPr>
                        <w:r>
                            <w:t xml:space=""preserve"">3 louis</w:t>
                        </w:r>
                    </w:p>
                </w:body>";

            var node = TestHelper.ParseString(input);

            Assert.AreEqual(3, node.Children.Count);
        }
    }
}