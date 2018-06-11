using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using OoxmlToHtml.Abstracts;
using OoxmlToHtml.Analyzers;
using OoxmlToHtml.test.Helpers;

namespace OoxmlToHtml.test.Analyzers
{
    [TestFixture]
    public class AttributeCopierAnalyzerTest
    {
        private INode _paragraph;
        private INode _pParagraphProp;
        [SetUp]
        public void Setup()
        {
            _paragraph = new Node(KeywordToken.Paragraph);
            _pParagraphProp = new Node(KeywordToken.PreviousParagraph);
            _pParagraphProp.SetAttribute("testAttr", "testAttrValue");
            _pParagraphProp.SetAttribute("testAttr2", "testAttr2Value");

            _paragraph.AddChild(_pParagraphProp);

        }

        [Test]
        public void ShouldTransferAttributesToParentNode()
        {
            var attributeCopierAnalyzer = new AttributeCopierAnalyzer();
            attributeCopierAnalyzer.Analyze(_paragraph);

            Assert.AreEqual("testAttrValue", _paragraph.GetAttribute("testAttr"));
            Assert.AreEqual("testAttr2Value", _paragraph.GetAttribute("testAttr2"));
        }

        [Test]
        public void ShouldNotTransferAttributesToParentNodeWhenNotAnalyzed()
        {
            Mock<AttributeCopierAnalyzer> mockedAnalyzer = new Mock<AttributeCopierAnalyzer>();
            mockedAnalyzer.CallBase = true;
            mockedAnalyzer.Setup(z => z.ShouldAnalyze(It.IsAny<INode>())).Returns(false);
            var attributeCopierAnalyzer = mockedAnalyzer.Object;
            attributeCopierAnalyzer.Analyze(_paragraph);

            Assert.Throws<KeyNotFoundException>(() => _paragraph.GetAttribute("testAttr"));
            Assert.Throws<KeyNotFoundException>(() => _paragraph.GetAttribute("testAttr2"));
        }

        [Test]
        public void ShouldNotTransferAttributesPastParagraph()
        {
            var attributeCopierAnalyzer = new AttributeCopierAnalyzer();

            var body = new Node(KeywordToken.Body);
            body.AddChild(_paragraph);
            attributeCopierAnalyzer.Analyze(body);

            Assert.Throws<KeyNotFoundException>(() => body.GetAttribute("testAttr"));
            Assert.AreEqual("testAttr2Value", body.Children[0].GetAttribute("testAttr2"));
            Assert.AreEqual("testAttrValue", body.Children[0].GetAttribute("testAttr"));
        }

        [Test]
        public void ShouldNotTransferAttributesPastRun()
        {
            var input = @"
                <w:body>
                    <w:p>
                        <w:r>                            
                            <w:color w:val=""538135""/>
                        </w:r>
                    </w:p>
                </w:body>
            ";
            var attributeCopierAnalyzer = new AttributeCopierAnalyzer();

            var node = TestHelper.ParseString(input);
            var result = attributeCopierAnalyzer.Analyze(node);

            Assert.Throws<KeyNotFoundException>(() =>  result.GetAttribute("value"));
            Assert.AreEqual("538135", result
                .Children.First()
                .GetAttribute("value"));
        }

        [Test]
        public void ShouldNotTransferAttributesPastParagraph2Levelsdeep()
        {
            var attributeCopierAnalyzer = new AttributeCopierAnalyzer();

            var body = new Node(KeywordToken.Body);
            var body2 = new Node(KeywordToken.Body);

            body.AddChild(body2);
            body2.AddChild(_paragraph);
            attributeCopierAnalyzer.Analyze(body);

            Assert.Throws<KeyNotFoundException>(() => body.GetAttribute("testAttr"));
            Assert.Throws<KeyNotFoundException>(() => body2.GetAttribute("testAttr"));
            Assert.AreEqual("testAttr2Value", body.Children[0].Children[0].GetAttribute("testAttr2"));
            Assert.AreEqual("testAttrValue", body.Children[0].Children[0].GetAttribute("testAttr"));
        }

        [Test]
        public void ShouldTransferChildrenAttributesToParentNode()
        {
            var pParagraphPropChild = new Node(KeywordToken.Color);

            _pParagraphProp.AddChild(pParagraphPropChild);
            pParagraphPropChild.SetAttribute("testAttr", "testAttrValue");
            _pParagraphProp.SetAttribute("testAttr2", "testAttr2Value");

            _paragraph.AddChild(_pParagraphProp);

            var attributeCopierAnalyzer = new AttributeCopierAnalyzer();
            attributeCopierAnalyzer.Analyze(_paragraph);

            Assert.AreEqual("testAttrValue", _paragraph.GetAttribute("testAttr"));
            Assert.AreEqual("testAttr2Value", _paragraph.GetAttribute("testAttr2"));
        }

        [Test]
        public void ShouldDeletePPrChildrenFromParentNode()
        {
            var pParagraphPropChild = new Node(KeywordToken.Color);

            _pParagraphProp.AddChild(pParagraphPropChild);
            _paragraph.AddChild(_pParagraphProp);

            var attributeCopierAnalyzer = new AttributeCopierAnalyzer();
            attributeCopierAnalyzer.Analyze(_paragraph);

            Assert.AreEqual(0, _paragraph.Children.Count);
            Assert.AreEqual(0, _pParagraphProp.Children.Count);
        }
    }
}