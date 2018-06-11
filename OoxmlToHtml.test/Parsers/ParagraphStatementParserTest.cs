using System.Runtime.InteropServices;
using NUnit.Framework;
using OoxmlToHtml.Abstracts;
using OoxmlToHtml.Parsers;
using OoxmlToHtml.test.Helpers;

namespace OoxmlToHtml.test.Parsers
{
    [TestFixture]
    public class ParagraphStatementParserTest
    {
        [Test]
        public void ShouldCreateBaseParagraphNode()
        {
            var result = ParseString(@"<w:p>
                                    <w:pPr>
                                        <w:jc w:val=""center""/>
                                        <w:rPr>
                                            <w:i/>
                                        </w:rPr>
                                    </w:pPr>
                                    <w:r>
                                        <w:rPr>
                                            <w:i/>
                                        </w:rPr>
                                        <w:t xml:space=""preserve"">The quick brown fox jumped … </w:t>
                                    </w:r>
                                  </w:p>");
            Assert.AreEqual(KeywordToken.Paragraph, result.Type);
        }

        [Test]
        public void ShouldParseUnknownStringLiteralInAttributesAsProperty()
        {
            var result = ParseString(@"<w:p someProp=""test"">
                                        </w:p>");

            Assert.AreEqual("test", result.GetAttribute("unknown"));
        }

        [Test]
        public void ShouldParseValueInAttributesAsProperty()
        {
            var result = ParseString(@"<w:p w:val=""test"">
                                        </w:p>");

            Assert.AreEqual("test", result.GetAttribute("value"));
        }
        
        [Test]
        public void ShouldParseColorAttributesToColorProperty()
        {
            var result = TestHelper.ParseString(@"<w:p w:val=""test"">
                                            <w:i />
                                            <w:color w:val=""FF0000"" />
                                        </w:p>");
            var colorChild = result.Children[1];
            Assert.AreEqual("FF0000", colorChild.GetAttribute("value"));
        }

        private INode ParseString(string text)
        {
            OoxmlNodeTd parent = new OoxmlNodeTd(new OoxmlScanner(new Source(text)));
            
            parent.Parse();
            return parent.Root.Root;
        }
    }
}