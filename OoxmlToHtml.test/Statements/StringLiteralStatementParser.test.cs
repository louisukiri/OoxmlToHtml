using NUnit.Framework;
using OoxmlToHtml.test.Helpers;

namespace OoxmlToHtml.test.Statements
{
    [TestFixture]
    public class StringLiteralStatementParser
    {
        [Test]
        public void ShouldExpandTokenToIncludeAdjacentStringLiterals()
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
                            <w:t xml:space=""preserve"">def louis</w:t>
                        </w:r>
                    </w:p>
                </w:body>";

            var node = TestHelper.ParseString(input);

            var stringLiteralNode = node.Children[0].Children[1].Children[0].Children[0];
            var stringLiteralNode2 = node.Children[1].Children[1].Children[0].Children[0];

            Assert.AreEqual("Abc louis", stringLiteralNode.GetAttribute("Text"));
            Assert.AreEqual("def louis", stringLiteralNode2.GetAttribute("Text"));
        }
        
        [TestCase(',')]
        [TestCase('-')]
        [TestCase('!')]
        [TestCase('(')]
        [TestCase(')')]
        [TestCase(':')]
        [TestCase('.')]
        public void ShouldIncludeSomeSpecialCharacters(char specialChar)
        {

            var input = @"<w:body>
                    <w:p>
                        <w:r>                            
                            <w:t xml:space=""preserve"">Abc" + specialChar + @" louis</w:t>
                        </w:r>
                    </w:p>
                </w:body>";

            var node = TestHelper.ParseString(input);

            var stringLiteralNode = node.child.child.child.child;

            Assert.AreEqual("Abc"+ specialChar +" louis", stringLiteralNode.GetAttribute("Text"));;
        }
    }
}