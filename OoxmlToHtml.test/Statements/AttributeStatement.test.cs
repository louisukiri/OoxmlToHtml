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
    public class AttributeStatementTest
    {
        [Test]
        public void ShouldParseAsExpectedAttribute()
        {
            var input = @"
                        <test w:val=""Title""/>
";
            var actual = new OoXmlParser(input).ParseProgram().Statements.First() as AttributeStatement;

            Assert.IsNotNull(actual);
            Assert.AreEqual("Title", actual?.TokenLiteral());
            Assert.AreEqual("Title", actual?.Value);
        }
    }
}
