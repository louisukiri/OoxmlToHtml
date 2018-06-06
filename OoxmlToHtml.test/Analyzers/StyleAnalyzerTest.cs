using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using OoxmlToHtml.Abstracts;
using OoxmlToHtml.Analyzers;

namespace OoxmlToHtml.test.Analyzers
{
    [TestFixture]
    public class StyleAnalyzerTest
    {
        [Test]
        public void ShouldTransferAttributesToParentNode()
        {
            var paragraph = new Node(KeywordToken.Paragraph);
            var pParagraphProp = new Node(KeywordToken.PreviousParagraph);
            pParagraphProp.SetAttribute("testAttr", "testAttrValue");
            pParagraphProp.SetAttribute("testAttr2", "testAttr2Value");

            paragraph.AddChild(pParagraphProp);

            var styleAnalyzer = new StyleAnalyzer();
            styleAnalyzer.Analyze(paragraph);

            Assert.AreEqual("testAttrValue", paragraph.GetAttribute("testAttr"));
            Assert.AreEqual("testAttr2Value", paragraph.GetAttribute("testAttr2"));
        }


        [Test]
        public void ShouldNotTransferAttributesToParentNodeWhenNotAnalyzed()
        {
            var paragraph = new Node(KeywordToken.Paragraph);
            var pParagraphProp = new Node(KeywordToken.PreviousParagraph);
            pParagraphProp.SetAttribute("testAttr", "testAttrValue");
            pParagraphProp.SetAttribute("testAttr2", "testAttr2Value");

            paragraph.AddChild(pParagraphProp);


            Mock<StyleAnalyzer> mockedAnalyzer = new Mock<StyleAnalyzer>(MockBehavior.Strict);
            mockedAnalyzer.Setup(z => z.ShouldAnalyze(It.IsAny<INode>())).Returns(false);
            var styleAnalyzer = mockedAnalyzer.Object;
            styleAnalyzer.Analyze(paragraph);

            Assert.Throws<KeyNotFoundException>(() => paragraph.GetAttribute("testAttr"));
            Assert.Throws<KeyNotFoundException>(() => paragraph.GetAttribute("testAttr2"));
        }

        [Test]
        public void ShouldTransferChildrenAttributesToParentNode()
        {
            var paragraph = new Node(KeywordToken.Paragraph);
            var pParagraphProp = new Node(KeywordToken.PreviousParagraph);
            var pParagraphPropChild = new Node(KeywordToken.Color);

            pParagraphProp.AddChild(pParagraphPropChild);
            pParagraphPropChild.SetAttribute("testAttr", "testAttrValue");
            pParagraphProp.SetAttribute("testAttr2", "testAttr2Value");

            paragraph.AddChild(pParagraphProp);

            var styleAnalyzer = new StyleAnalyzer();
            styleAnalyzer.Analyze(paragraph);

            Assert.AreEqual("testAttrValue", paragraph.GetAttribute("testAttr"));
            Assert.AreEqual("testAttr2Value", paragraph.GetAttribute("testAttr2"));
        }

        [Test]
        public void ShouldDeletePPrChildrenFromParentNode()
        {
            var paragraph = new Node(KeywordToken.Paragraph);
            var pParagraphProp = new Node(KeywordToken.PreviousParagraph);
            var pParagraphPropChild = new Node(KeywordToken.Color);

            pParagraphProp.AddChild(pParagraphPropChild);
            paragraph.AddChild(pParagraphProp);

            var styleAnalyzer = new StyleAnalyzer();
            styleAnalyzer.Analyze(paragraph);

            Assert.AreEqual(0, paragraph.Children.Count);
            Assert.AreEqual(0, pParagraphProp.Children.Count);
        }
    }
}