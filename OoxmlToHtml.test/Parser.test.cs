using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Moq;
using OoxmlToHtml.Abstracts;
using OoxmlToHtml.Analyzers;

namespace OoxmlToHtml.test
{
    [TestFixture]
    public class ParserTest
    {
        private string _input;
        [SetUp]
        public void Setup()
        {
            _input = @"
                    <w:color w:val=""FF0000""/>
                    <w:p>
                        <w:pPr>
                            <w:rPr>
                                <w:i/>
                            </w:rPr>
                        </w:pPr>
                        <w:r>
                            <w:rPr>
                                <w:i/>
                            </w:rPr>
                            <w:t>The quick brown fox jumped … </w:t>
                        </w:r>
                     </w:p>
                    <w:sz w:val=""16"" />
";
            //_lexer = new Lexer(_input);
            //_ooXmlParser = new OoXmlParser(_lexer);
        }

        [Test]
        public void ShouldRunsAnalyzersOnParse()
        {
            var analyzer = new Mock<Analyzer>(MockBehavior.Strict);
            analyzer.Setup(z => z.Analyze(It.IsAny<INode>()))
                .Returns<INode>(n => n);

            var testInput = @"<w:p testAttrib=""test"">ok jim</w:p>";
            var a = new OoxmlNodeTd(new OoxmlScanner(new Source(testInput)));
            a.Use(analyzer.Object);
            a.Parse();

            analyzer.Verify(z => z.Analyze(It.IsAny<INode>()));
        }

        [Test]
        public void ParseDoesRunThroughDefaultAnalyzersWhenSpecified()
        {
            var analyzer = new Mock<Analyzer>(MockBehavior.Strict);
            analyzer.Setup(z => z.Analyze(It.IsAny<INode>()))
                .Returns<INode>(n => n);

            var testInput = @"<w:p testAttrib=""test"">
                                <w:pPr>
                                    <w:b />
                                </w:pPr>
                                <w:r>
                                    <w:t>ok jim</w:t>
                                </w:r>
                              </w:p>";
            var a = new OoxmlNodeTd(new OoxmlScanner(new Source(testInput)));
            a.Parse(true);

            Assert.AreEqual(bool.TrueString, a.Root.Root.GetAttribute("bold"));
        }

        [Test]
        public void ParsePassesDefaultAnalyzersResultsToCustomAnalyzers()
        {
            var analyzer = new Mock<Analyzer>(MockBehavior.Strict);
            analyzer.Setup(z => z.Analyze(It.IsAny<INode>()))
                .Returns<INode>(n =>
                {
                    Assert.AreEqual(bool.TrueString, n.GetAttribute("bold"));
                    return n;
                });

            var testInput = @"<w:p testAttrib=""test"">
                                <w:pPr>
                                    <w:b />
                                </w:pPr>
                              </w:p>";
            var a = new OoxmlNodeTd(new OoxmlScanner(new Source(testInput)));
            a.Use(analyzer.Object);
            a.Parse(true);
            
        }

        [Test]
        public void ParseReturnsBodyAsRootNodeWhereAppropriate()
        {
            // new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.FullName
            var pathCurrentDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory)
                .Parent?
                .Parent?
                .FullName + "/Data/RootNodeTest.xml";

            var testInput = File.ReadAllText(pathCurrentDirectory);
            var a = new OoxmlNodeTd(new OoxmlScanner(new Source(testInput)));
            a.Parse(true);

            Assert.AreEqual(KeywordToken.Body, a.Root.Root.Type);
        }


        [Test]
        public void ParseWillNotParseAnyTagsOutsideOfW_Document()
        {
            var testInput = @"<pkg:package>
                                <pkg:xmlData><w:document>
                                <w:body>
                                    <w:p testAttrib=""test"">
                                    <w:pPr>
                                        <w:b />
                                    </w:pPr>
                                    </w:p>
                            </w:body></w:document></pkg:xmlData>
                            <pkg:part pkg:name=""/word/footnotes.xml"" pkg:contentType=""application/vnd.openxmlformats-officedocument.wordprocessingml.footnotes+xml"">
                                <pkg:xmlData>
                                    <w:footnote w:type=""separator"" w:id="" - 1"">
                                        <w:p w:rsidR = ""00644911"" w:rsidRDefault = ""00644911"" w:rsidP = ""00230A5E"">
                                            <w:pPr>
                                                <w:spacing w:after=""0"" w:line = ""240"" w:lineRule=""auto"" />
                                            </ w:pPr >
                                            <w:r>                   
                                                <w:separator/>
                                            </w:r>
                                        </w:p>
                                    </w:footnote>                      
                                </pkg:xmlData>
                            <pkg:part>
                          </pkg:package>
";
            var a = new OoxmlNodeTd(new OoxmlScanner(new Source(testInput)));
            a.Parse(true);

            Assert.AreEqual(KeywordToken.Body, a.Root.Root.Type);
        }

        [Test]
        public void ParseDoesNotRunThroughDefaultAnalyzersWhenSpecified()
        {
            var analyzer = new Mock<Analyzer>(MockBehavior.Strict);
            analyzer.Setup(z => z.Analyze(It.IsAny<INode>()))
                .Returns<INode>(n => n);

            var testInput = @"<w:p testAttrib=""test"">
                                <w:pPr>
                                    <w:b />
                                </w:pPr>
                                <w:r>
                                    <w:t>ok jim</w:t>
                                </w:r>
                              </w:p>";
            var a = new OoxmlNodeTd(new OoxmlScanner(new Source(testInput)));
            a.Parse();

            Assert.Throws<KeyNotFoundException>(() => a.Root.Root.GetAttribute("bold"));
        }
        #region parsing test

        [Test]
        public void CurrentTokenDoesNotMoveCharForward()
        {
            var testInput = @"<w:p testAttrib=""test"">ok jim</w:p>";
            var a = new OoxmlNodeTd(new OoxmlScanner(new Source(testInput)));
            a.NextToken();
            Assert.AreEqual(a.CurrentToken.Keyword, KeywordToken.STARTING_ELEMENT);
            a.NextToken();
            Assert.AreEqual(a.CurrentToken.Keyword, KeywordToken.Paragraph);

            Assert.AreEqual(a.CurrentToken.Keyword, KeywordToken.Paragraph);
        }
        [Test]
        public void TestBasicNode()
        {
            var testInput = @"<w:p testAttrib=""test"" another=""testagain"">ok jim</w:p>";

            var program = new OoxmlNodeTd(new OoxmlScanner(new Source(testInput)));
            program.Parse();

            var rootNode = program.Root.Root;
            Assert.AreEqual(KeywordToken.Paragraph, rootNode.Type);
            Assert.AreEqual("test", rootNode.GetAttribute("unknown"));
            Assert.AreEqual("testagain",rootNode.GetAttribute("unknown_2"));
            Assert.AreEqual("ok jim", rootNode.Children.First().GetAttribute("Text"));
        }

        [Test]
        public void TestContainerElementInsideContainerElement()
        {
            var testInput = @"<w:p>
                                <w:pPr>
                                    <w:color w:val=""FF0000"" />
                                </w:pPr>
                                <w:t>testing me too</w:t>
                              </w:p>";

            var program = new OoxmlNodeTd(new OoxmlScanner(new Source(testInput)));
            program.Parse();

            var rootNode = program.Root.Root;
            Assert.AreEqual(KeywordToken.Paragraph, rootNode.Type);
            //Assert.AreEqual(KeywordToken.Color, rootNode.Children[0].Children[0].Type);
            // Assert.AreEqual("FF0000", rootNode.GetAttribute("fontColor"));
            //Assert.AreEqual(KeywordToken.Text, rootNode.Children[1].Type);
            //Assert.AreEqual("testing me too", rootNode.Children[1].Children[0].GetAttribute("value"));
            //Assert.AreEqual(2, rootNode.Children.Count);
        }
#endregion
    }
}
