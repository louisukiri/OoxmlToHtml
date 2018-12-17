using System;
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
        public void ShouldNotCollapseChildOfTheSameTypeAsParentIntoParent()
        {
            var result = TestHelper.ParseString(@"<w:body>
                                                    <w:p w:val=""test"">
                                                        <w:p>
                                                            <w:t>Te</w:t>
                                                        </w:p>
                                                        <w:p w:rsidRPr=""00C63EA3"">
                                                            <w:t>sting this testing thing</w:t>
                                                        </w:p>
                                                        <w:p></w:p>
                                                    </w:p>
                                                  </w:body>");
            var paragraphChild = result.Children[0];
            Console.WriteLine(result.ToString());
            Assert.AreEqual(3, paragraphChild.Children.Count);
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
                                                    <w:p w:rsidRDefault = ""001F0010"" w:rsidR = ""00000000"">
                                                        <w:r>
                                                            <w:t> In this simple text entry </ w:t>
                                                        </w:r>
                                                    </w:p></w:body>", true);
            var paragraphChild = result.Children.First();
            Assert.AreEqual("Title", paragraphChild.GetAttribute("style"));
        }
        [Test]
        public void ParseShouldLeaveCursorOneTokenPastTheParagraphBoundary()
        {
            var parserNode = new OoxmlNodeTd(new OoxmlScanner(
                new Source(@"<w:body>
                                <w:p>
                                    <w:t>Testing this string</w:t>
                                </w:p>
                            </w:body>")
            ));
            parserNode.NextToken();
            while (parserNode.CurrentToken.Keyword != KeywordToken.Paragraph)
            {
                parserNode.NextToken();
            }
            var sut = new ParagraphStatementParser(parserNode);
            sut.Parse(parserNode.CurrentToken);

            var currentToken = parserNode.CurrentToken;
            Assert.AreEqual(KeywordToken.Close, currentToken.Keyword);
            Assert.AreEqual("w:body", currentToken.Text);
        }
    }
}