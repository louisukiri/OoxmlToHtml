﻿
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
                new Tokens(Tokens.Color, "w:color"),
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
                </w:p>
";
            var expected = @"<p style=""font-weight: bold; font-style: italic; color:#538135; font-size: 16px; ""></p>";
            var l = new Lexer(input);
            var p = new Parser(l);
            var program = p.ParseProgram();
            var htmlVisitor = new HtmlVisitor();
            htmlVisitor.Visit(program);

            Assert.AreEqual(expected, htmlVisitor.Value);
        }
    }
}