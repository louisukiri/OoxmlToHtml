using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;
using OoxmlToHtml.Statements;

namespace OoxmlToHtml.test
{
    [TestFixture]
    public class ParserTest
    {
        [Test]
        public void TestBasicNode()
        {
            const string input = @"
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
            var l = new Lexer(input);
            var p = new Parser(l);
            var program = p.ParseProgram();

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
    }
}
