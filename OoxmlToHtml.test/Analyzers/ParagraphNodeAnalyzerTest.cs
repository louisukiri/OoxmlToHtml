using NUnit.Framework;
using OoxmlToHtml.Analyzers;

namespace OoxmlToHtml.test.Analyzers
{
    [TestFixture]
    public class ParagraphNodeAnalyzerTest
    {
        [Test]
        public void ShouldAnalyzePreviousParagraph()
        {
            var paragraph = new Node(KeywordToken.Paragraph);
            var pParagraphProp = new Node(KeywordToken.PreviousParagraph);
            pParagraphProp.SetAttribute("testAttr", "testAttrValue");
            pParagraphProp.SetAttribute("testAttr2", "testAttr2Value");

            paragraph.AddChild(pParagraphProp);

            var styleAnalyzer = new ParagraphNodeAnalyzer();
            styleAnalyzer.Analyze(paragraph);

            Assert.AreEqual("testAttrValue", paragraph.GetAttribute("testAttr"));
            Assert.AreEqual("testAttr2Value", paragraph.GetAttribute("testAttr2"));
        }
    }
}