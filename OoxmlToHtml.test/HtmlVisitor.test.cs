using System;
using NUnit.Framework;
using OoxmlToHtml.Printers;
using OoxmlToHtml.test.Helpers;

namespace OoxmlToHtml.test
{
    [TestFixture]
    public class HtmlPrinterTest
    {
        [Test]
        public void ParagraphTest()
        {
            var input = @"
              <w:body>
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
            </w:body>
";
            var expected = @"<div style=""font-weight:bold;font-style:italic;color:#538135;font-size:16px;""><span>Abc louis</span><span style=""font-weight:bold;"">def</span><span>ijk</span></div>";
            var l = TestHelper.ParseString(input);
            var p = new HtmlPrinter();
            p.Print(l);
            Console.WriteLine(p.HtmlString);
            Assert.AreEqual(expected, p.HtmlString);
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
            var expected = @"<div><h1><span>Title</span></h1></div>";
            var l = TestHelper.ParseString(input);
            var p = new HtmlPrinter();
            p.Print(l);

            Assert.AreEqual(expected, p.HtmlString);
        }

    }
}
