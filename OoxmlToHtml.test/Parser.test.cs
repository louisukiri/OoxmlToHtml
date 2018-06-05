using System.Linq;
using NUnit.Framework;
using OoxmlToHtml.Statements;
using Moq;
using OoxmlToHtml.Abstracts;

namespace OoxmlToHtml.test
{
    [TestFixture]
    public class ParserTest
    {
        private string _input;
        private Lexer _lexer;
        private OoXmlParser _ooXmlParser;
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
        //[Test]
        //public void AnalyzeRunsThroughAllAnalyzers()
        //{
        //    var program = new Mock<IProgram>();
        //    var analyzer = new Mock<IAnalyzer>(MockBehavior.Strict);
        //    analyzer.Setup(z => z.Analyze(It.IsAny<IProgram>()))
        //        .Returns(
        //            program.Object
        //        );
        //    _ooXmlParser.Use(analyzer.Object);

        //    _ooXmlParser.Analyze(program.Object);

        //    analyzer.Verify(z => z.Analyze(It.IsAny<IProgram>()));
        //}

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
            Assert.AreEqual(1, rootNode.Children.Count);
            //var program = _ooXmlParser.ParseProgram();

            //if (program == null)
            //{
            //    Assert.Fail("ParseProgram() returned null");
            //}

            //if (program.Statements.Count() != 3)
            //{
            //    Assert.Fail("Invalid number of statements in program");
            //}

            //TestParagraphStatement(program.Statements[1] as ParagraphStatement, 2);
            //TestSizeStatement(program.Statements[2] as SizeStatement, "16");
            //var tests = new string[]
            //{
            //    "w:color"
            //};

            //for (var i = 0; i < tests.Length; i++)
            //{
            //    var test = tests[i];
            //    var statement = program.Statements[i] as ColorStatement;
            //    if (!TestColorStatement(statement, test, "FF0000"))
            //    {
            //        return;
            //    }
            //}
        }

        private bool TestColorStatement(ColorStatement statement, string name, string value)
        {
            if (statement.Token.Literal != name)
            {
                Assert.Fail("s.TokenLiteral not 'color'");
            }

            if (statement.Value != value)
            {
                Assert.Fail("Statement's value is incorrect");
            }

            return true;
        }

        private bool TestSizeStatement(SizeStatement statement, string size)
        {
            if (statement.TokenLiteral() != size)
            {
                Assert.Fail("Invalid value ({0}) for size statement", size);
            }

            return true;
        }

        private bool TestParagraphStatement(ParagraphStatement statement, int numberOfChildren)
        {

            if (statement.Statements.Count() != 2)
            {
                Assert.Fail("Invalid number of child statements in paragraph statements");
            }

            return true;
        }
#endregion
    }
}
