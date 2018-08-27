using System;
using System.Linq;
using NUnit.Framework;
using OoxmlToHtml.Analyzers;
using OoxmlToHtml.test.Helpers;

namespace OoxmlToHtml.test.Analyzers
{
    public class SiblingsAnalyzerTest
    {
        [Test]
        public void ShouldMergeSiblingsOfSameType()
        {
            var node = TestHelper.ParseString(@"<w:body>
                                                    <w:p w:rsidP=""001F0010"" w:rsidRDefault=""001F0010"" w:rsidR=""001F0010"">
                                                        <w:pPr><w:pStyle w:val = ""Title""/></w:pPr>
                                                        <w:r>
                                                            <w:rPr>
                                                                <w:rStyle w:val=""TitleChar""/>
                                                            </w:rPr>
                                                            <w:t>Te</w:t>
                                                        </w:r>
                                                        <w:r w:rsidRPr=""00C63EA3"">
                                                            <w:rPr>
                                                                <w:rStyle w:val=""TitleChar2""/>
                                                            </w:rPr>
                                                            <w:t>sting this testing thing</w:t>
                                                        </w:r>
                                                        <w:r>
                                                            <w:rPr>
                                                                <w:rStyle w:val=""TitleChar3""/>
                                                            </w:rPr>
                                                            <w:t>2</w:t>
                                                        </w:r>
                                                    </w:p>
                                                </w:body>");
            var siblingsAnalyzer = new SiblingsAnalyzer();
            var result = siblingsAnalyzer.Analyze(node);
            var paragraphChild = result.Children.First();


            Console.WriteLine(result.ToString());
            Assert.AreEqual(2, paragraphChild.Children.Count);
            Assert.AreEqual(KeywordToken.Text, paragraphChild.Children[1].Children[3].Type);
            Assert.AreEqual(6, paragraphChild.Children[1].Children.Count);
        }
        [Test]
        public void ShouldNotMergeSiblingsOfTypeParagraph()
        {
            var node = TestHelper.ParseString(@"<w:body>
                                                    <w:p w:rsidP=""001F0010"" w:rsidRDefault=""001F0010"" w:rsidR=""001F0010"">
                                                        <w:pPr><w:pStyle w:val = ""Title""/></w:pPr>
                                                        <w:p>
                                                            <w:t>Te</w:t>
                                                        </w:p>
                                                        <w:p w:rsidRPr=""00C63EA3"">
                                                            <w:t>sting this testing thing</w:t>
                                                        </w:p>
                                                        <w:p></w:p>
                                                    </w:p>
                                                </w:body>");
            var nodeXml = node.ToString();
            var siblingsAnalyzer = new SiblingsAnalyzer();
            var result = siblingsAnalyzer.Analyze(node);


            // nothing's changed
            Assert.AreEqual(result.ToString(), nodeXml);
        }
    }
}