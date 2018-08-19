using System.Linq;
using NUnit.Framework;
using OoxmlToHtml.Abstracts;
using OoxmlToHtml.Analyzers;
using OoxmlToHtml.test.Helpers;

namespace OoxmlToHtml.test.Analyzers
{
    public class ChildrenAnalyzerTest
    {
        [SetUp]
        public void Setup()
        {
        }
        [Test]
        public void ShouldNotHaveImmediateChildOfSameType()
        {
            var node = TestHelper.ParseString(@"<w:body>
                                                    <w:p w:rsidP=""001F0010"" w:rsidRDefault=""001F0010"" w:rsidR=""001F0010"">
                                                        <w:pPr><w:pStyle w:val = ""Title""/></w:pPr>
                                                        <w:p>
                                                            <w:t>TestTItle</w:t>
                                                         </w:p>
                                                    </w:p>
                                                </w:body>");
            var childrenAnalyzer = new ChildrenAnalyzer();
            var result = childrenAnalyzer.Analyze(node);
            var paragraphChild = result.Children.First();

            Assert.AreEqual(2, paragraphChild.Children.Count);
            Assert.AreEqual(KeywordToken.Text, paragraphChild.Children[1].Type);
        }
        [Test]
        public void ShouldNotMergeNodesOfTheSameTypeIfPropsConflict()
        {
            var node = TestHelper.ParseString(@"<w:body>
                                                    <w:p test=""oldValue"">
                                                        <w:pPr><w:pStyle w:val = ""Title""/></w:pPr>
                                                        <w:p test=""newValue"">
                                                            <w:t>TestTItle</w:t>
                                                         </w:p>
                                                    </w:p>
                                                </w:body>");
            var childrenAnalyzer = new ChildrenAnalyzer();
            var result = childrenAnalyzer.Analyze(node);
            var paragraphChild = result.Children.First();

            Assert.AreEqual(2, paragraphChild.Children.Count);
            Assert.AreEqual(KeywordToken.Paragraph, paragraphChild.Children[1].Type);
        }
    }
}