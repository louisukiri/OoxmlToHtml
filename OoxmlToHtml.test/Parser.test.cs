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
        private Parser _parser;
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
            _lexer = new Lexer(_input);
            _parser = new Parser(_lexer);
        }
        [Test]
        public void AnalyzeRunsThroughAllAnalyzers()
        {
            var analyzer = new Mock<IAnalyzer>(MockBehavior.Strict);
            analyzer.Setup(z => z.Analyze(It.IsAny<IProgram>()))
                .Returns(
                    new Mock<IAnalysisResult>().Object
                );
            _parser.Use(analyzer.Object);

            _parser.Analyze();

            analyzer.Verify(z => z.Analyze(It.IsAny<IProgram>()));
        }

#region parsing test
        [Test]
        public void TestBasicNode()
        {
            var program = _parser.ParseProgram();

            if (program == null)
            {
                Assert.Fail("ParseProgram() returned null");
            }

            if (program.Statements.Count() != 3)
            {
                Assert.Fail("Invalid number of statements in program");
            }

            TestParagraphStatement(program.Statements[1] as ParagraphStatement, 2);
            TestSizeStatement(program.Statements[2] as SizeStatement, "16");
            var tests = new string[]
            {
                "w:color"
            };

            for (var i = 0; i < tests.Length; i++)
            {
                var test = tests[i];
                var statement = program.Statements[i] as ColorStatement;
                if (!TestColorStatement(statement, test, "FF0000"))
                {
                    return;
                }
            }
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
