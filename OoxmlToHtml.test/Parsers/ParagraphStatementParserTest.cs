using System.Runtime.InteropServices;
using NUnit.Framework;
using OoxmlToHtml.Abstracts;
using OoxmlToHtml.Parsers;

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
        public void ShouldParseItalicInAttributesToBooleanProperty()
        {
            var result = ParseString(@"<w:p w:val=""test"">
                                            <w:i />
                                        </w:p>");

            Assert.AreEqual(bool.TrueString, result.GetAttribute("italic"));
        }

        [Test]
        public void ShouldParseColorAttributesToColorProperty()
        {
            var result = ParseString(@"<w:p w:val=""test"">
                                            <w:i />
                                            <w:color w:val=""FF0000"" />
                                        </w:p>");

            Assert.AreEqual("FF0000", result.GetAttribute("fontColor"));
        }

        private INode ParseString(string text)
        {
            OoxmlNodeTd parent = new OoxmlNodeTd(new OoxmlScanner(new Source(text)));
            //var parser = new ParagraphStatementParser(parent);

            //return parser.Parse(parser.CurrentToken);
            parent.Parse();
            return parent.Root.Root;
        }
    }
}