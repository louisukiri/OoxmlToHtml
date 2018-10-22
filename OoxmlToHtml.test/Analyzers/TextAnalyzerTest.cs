using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OoxmlToHtml.Abstracts;
using OoxmlToHtml.Analyzers;
using OoxmlToHtml.test.Helpers;

namespace OoxmlToHtml.test.Analyzers
{
    [TestFixture]
    public class TextAnalyzerTest
    {
        private INode result;
        [SetUp]
        public void Setup()
        {
            var input = @"
                <w:body>
                    <w:p>
                        <w:r>                            
                            <w:t> ok jim, ok</w:t>
                        </w:r>
                    </w:p>
                </w:body>
            ";
            var textAnalyzer = new TextAnalyzer();

            var node = TestHelper.ParseString(input, true);
            result = textAnalyzer.Analyze(node);
        }
        [Test]
        public void ShouldPropagateTextAttributeToRun()
        {
            var paragraphNode = result.Child.Child;
            Assert.AreEqual("ok jim, ok", paragraphNode.GetAttribute("Text"));
        }
    }
}