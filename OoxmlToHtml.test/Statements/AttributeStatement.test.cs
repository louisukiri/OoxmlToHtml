using System.Linq;
using NUnit.Framework;
using OoxmlToHtml.test.Helpers;

namespace OoxmlToHtml.test.Statements
{
    [TestFixture]
    public class AttributeStatementTest
    {
        [Test]
        public void ShouldParseAsExpectedAttribute()
        {
            var input = @"
<w:p>
                        <test w:val=""Title""/></w:p>
";
            var actual = TestHelper.ParseString(input);
            //var actual = new OoXmlParser(input).ParseProgram().Statements.First() as AttributeStatement;
            
            Assert.IsNotNull(actual);
            Assert.AreEqual("test", actual?.Children.First().GetAttribute("Text"));
            //Assert.AreEqual("Title", actual?.Value);
        }
    }
}
