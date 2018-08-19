using System.Linq;
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
            var result = TestHelper.ParseString(@"<w:p>
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
            var result = TestHelper.ParseString(@"<w:p someProp=""test"">
                                        </w:p>");

            Assert.AreEqual("test", result.GetAttribute("unknown"));
        }

        [Test]
        public void ShouldParseValueInAttributesAsProperty()
        {
            var result = TestHelper.ParseString(@"<w:p w:val=""test"">
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

        [Test]
        public void ShouldParseHeaderAttributesToAttributes()
        {
            var result = TestHelper.ParseString(@"<w:body>
                                                    <w:p w:rsidP=""001F0010"" w:rsidRDefault=""001F0010"" w:rsidR=""001F0010"">
                                                        <w:pPr><w:pStyle w:val = ""Title""/></w:pPr>
                                                        <w:r>
                                                            <w:t>TestTItle</w:t>
                                                         </w:r>
                                                    </w:p>
                                                    <w:p w:rsidRDefault = ""001F0010"" w: rsidR = ""00000000"">
                                                        <w:r>
                                                            <w:t> In this simple text entry </ w:t>
                                                        </w:r>
                                                    </w:p></w:body>", true);
            var paragraphChild = result.Children.First();
            Assert.AreEqual("Title", paragraphChild.GetAttribute("style"));
        }
    }
}