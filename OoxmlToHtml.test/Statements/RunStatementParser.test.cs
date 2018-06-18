using System.Linq;
using NUnit.Framework;
using OoxmlToHtml.test.Helpers;

namespace OoxmlToHtml.test.Statements
{
    [TestFixture]
    public class RunStatementParser
    {
        [Test]
        public void ShouldNotNestRuns()
        {
            var input = @"<w:body>
                    <w:p>
                        <w:r>                            
                            <w:t xml:space=""preserve"">Abc louis</w:t>
                        </w:r>
                        <w:r>
                            <w:t xml:space=""preserve"">22 louis</w:t>
                        </w:r>
                        <w:r>
                            <w:t xml:space=""preserve"">3 louis</w:t>
                        </w:r>
                    </w:p>
                </w:body>";

            var node = TestHelper.ParseString(input);

            Assert.AreEqual(3, node.Children.First().Children.Count);
        }
    }
}