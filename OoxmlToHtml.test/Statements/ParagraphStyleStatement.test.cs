using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OoxmlToHtml.Statements;
using OoxmlToHtml.test.Helpers;

namespace OoxmlToHtml.test.Statements
{
    [TestFixture]
    public class ParagraphStyleStatementTest
    {
        [Test]
        public void ShouldCorrectlyParseValue()
        {
            var input = @"<w:p>
                            <w:pStyle w:val=""Title""/>
                          </w:p>
";
            var actual = TestHelper.ParseString(input);
            
            Assert.IsNotNull(actual);
            Assert.AreEqual("Title", actual?.Children.First().GetAttribute("value"));
        }
    }
}
