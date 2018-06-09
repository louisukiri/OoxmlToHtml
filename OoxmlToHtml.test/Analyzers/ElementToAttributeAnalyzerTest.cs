using System.Collections.Generic;
using NUnit.Framework;
using OoxmlToHtml.Abstracts;
using OoxmlToHtml.Analyzers;
using OoxmlToHtml.test.Helpers;

namespace OoxmlToHtml.test.Analyzers
{
    [TestFixture]
    public class ElementToAttributeAnalyzerTest
    {
        private string _testInput;
        private INode _node, _result;
        ElementToAttributeAnalyzer _sut;

        [SetUp]
        public void Setup()
        {
            _testInput = @"<w:p testAttrib=""test"">
                                <w:pPr>
                                    <w:color w:val=""FF0000"" />
                                </w:pPr>
                              </w:p>";
            _node = TestHelper.ParseString(_testInput);
            _sut = new ElementToAttributeAnalyzer();
            _result = _sut.Analyze(_node);
        }
        [Test]
        public void ShouldConvertColorAttributeToFontColorOfParent()
        {
            Assert.AreEqual(_result
                .Children[0]
                .GetAttribute("fontColor"), "FF0000");
        }
        [Test]
        public void ShouldRemoveColorElementInConversion()
        {
            Assert.AreEqual(0, _result
                .Children[0]
                .Children.Count);
        }
    }
}