using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OoxmlToHtml.Statements;

namespace OoxmlToHtml.test.Statements
{
    [TestFixture]
    public class ParagraphStyleStatementTest
    {
        [Test]
        public void ShouldCorrectlyParseValue()
        {
            var input = @"
                        <w:pStyle w:val=""Title""/>
";
            var actual = new OoXmlParser(input).ParseProgram().Statements.First() as ParagraphStyleStatement;
            
            Assert.IsNotNull(actual);
            Assert.AreEqual("Title", actual?.TokenLiteral());
        }
    }
}
