using System.Collections.Generic;
using NUnit.Framework;
using OoxmlToHtml.Analyzers;
using OoxmlToHtml.test.Helpers;

namespace OoxmlToHtml.test.Analyzers
{
    [TestFixture]
    public class ElementToAttributeAnalyzerTest
    {
        [Test]
        public void ShouldHoistColorAttribute2LayersDeep()
        {
            var testInput = @"<w:p testAttrib=""test"">
                                <w:pPr>
                                    <w:color w:val=""FF0000"" />
                                </w:pPr>
                              </w:p>";
            var node = TestHelper.ParseString(testInput);
            var sut = new ElementToAttributeAnalyzer();
            var result = sut.Analyze(node);

            Assert.AreEqual(result.GetAttribute("fontColor"), "FF0000");
        }

        [Test]
        public void ShouldHoistColorAttributeSingleLayer()
        {
            var testInput = @"<w:p testAttrib=""test"">
                                    <w:color w:val=""FF0000"" />
                              </w:p>";
            var node = TestHelper.ParseString(testInput);
            var sut = new ElementToAttributeAnalyzer();
            var result = sut.Analyze(node);

            Assert.AreEqual(result.GetAttribute("fontColor"), "FF0000");
        }

        [Test]
        public void ShouldHoistColorAttribute3LayersDeep()
        {
            var testInput = @"<w:p testAttrib=""test"">
                                <w:pPr>
                                    <w:rPr>
                                        <w:color w:val=""FF0000"" />
                                    </w:rPr>
                                </w:pPr>
                              </w:p>";
            var node = TestHelper.ParseString(testInput);
            var sut = new ElementToAttributeAnalyzer();
            var result = sut.Analyze(node);

            Assert.AreEqual(result.GetAttribute("fontColor"), "FF0000");
        }

        [Test]
        public void ShouldNotHoistPastParagraph()
        {
            var testInput = @"<w:p><w:p testAttrib=""test"">
                                <w:pPr>
                                    <w:rPr>
                                        <w:color w:val=""FF0000"" />
                                    </w:rPr>
                                </w:pPr>
                              </w:p></w:p>";
            var node = TestHelper.ParseString(testInput);
            var sut = new ElementToAttributeAnalyzer();
            var result = sut.Analyze(node);

            Assert.Throws<KeyNotFoundException>(() => result.GetAttribute("fontColor"));
            Assert.AreEqual(result.Children[0].GetAttribute("fontColor"), "FF0000");
        }
    }
}