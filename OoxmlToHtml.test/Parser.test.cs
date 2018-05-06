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
";
            var l = new Lexer(input);
            var p = new Parser(l);
            var program = p.ParseProgram();

            if (program == null)
            {
                Assert.Fail("ParseProgram() returned null");
            }

            if (program.Statements.Count() != 1)
            {
                Assert.Fail("Invalid number of statements in program");
            }

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
            if (statement.TokenLiteral() != name)
            {
                Assert.Fail("s.TokenLiteral not 'color'");
            }

            if (statement.Value.TokenLiteral() != value)
            {
                Assert.Fail("Statement's value is incorrect");
            }

            return true;
        }
    }
}
