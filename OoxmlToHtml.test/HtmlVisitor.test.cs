
using NUnit.Framework;
using OoxmlToHtml.Statements;
using OoxmlToHtml.Visitors;

namespace OoxmlToHtml.test
{
    [TestFixture]
    public class HtmlVisitorTest
    {
        [Test]
        public void ColorTest()
        {
            var htmlVisitor = new HtmlVisitor();
            htmlVisitor.Visit(new ColorStatement(
                new Token(Token.Color, "w:color"),
                "ff000f"
                ));

            Assert.AreEqual("color:#ff000f; ", htmlVisitor.Value);
        }
        [Test]
        public void ParagraphTest()
        {
            var input = @"
                <w:p>
                    <w:pPr>
                        <w:rPr>
                            <w:b/>
                            <w:i/>
                            <w:color w:val=""538135"" w:themeColor=""accent6"" w:themeShade=""BF""/>
                            <w:sz w:val=""16""/>
                            <w:szCs w:val=""16""/>
                        </w:rPr>
                    </w:pPr>
                    <w:r>
                        <w:t xml:space=""preserve"">Abc louis</w:t>
                    </w:r>
                    <w:r>
                        <w:rPr>
                            <w:b/>
                        </w:rPr>
                        <w:t xml:space=""preserve"">def </w:t>
                    </w:r>
                    <w:r>
                        <w:t>ijk</w:t>
                    </w:r>
                </w:p>
";
            var expected = @"<p style=""font-weight: bold; font-style: italic; color:#538135; font-size: 16px; ""><span>Abc louis</span><span style=""font-weight: bold; "">def</span><span>ijk</span></p>";
            var l = new Lexer(input);
            var p = new OoXmlParser(l);

            Assert.AreEqual(expected, p.Parse().Value);
        }

        [Test]
        public void HeaderH1Test()
        {
            var input = @"
                <w:p>
                    <w:pPr>
                        <w:pStyle w:val=""Title""/>
                    </w:pPr>
                    <w:r>
                        <w:t>Title</w:t>
                    </w:r>
                </w:p>
";
            var expected = @"<p style=""""><h1><span>Title</span></h1></p>";
            var l = new Lexer(input);
            var p = new OoXmlParser(l);

            Assert.AreEqual(expected, p.Parse().Value);
        }

    }
}
