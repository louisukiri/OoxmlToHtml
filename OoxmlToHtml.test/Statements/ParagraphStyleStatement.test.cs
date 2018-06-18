using System.Linq;
using NUnit.Framework;
using OoxmlToHtml.test.Helpers;

namespace OoxmlToHtml.test.Statements
{
    [TestFixture]
    public class ParagraphStyleStatementTest
    {
        [Test]
        public void ShouldCorrectlyParseValue()
        {
            var input = @"<w:body><w:p>
                            <w:pStyle w:val=""Title""/>
                          </w:p></w:body>
";
            var actual = TestHelper.ParseString(input);

            Assert.IsNotNull(actual);
            Assert.AreEqual("Title", actual?
                .Children.First()
                .Children.First()
                    .GetAttribute("value"));
        }

        [Test]
        public void ShouldNotAddUnknownTagAsProp()
        {
            var input = @"
                            <w:p>
                                <w:unknown w:val=""unknown tag value"">test</w:unknown>
                            </w:p>
";
            var actual = TestHelper.ParseString(input);

            Assert.IsNotNull(actual);
            Assert.IsFalse(actual.HasAttribute("Text") && actual.GetAttribute("Text") == "w:unknown");
        }

        [Test]
        public void ShouldPropagateValuesAsPropForUnknownTag()
        {
            var input = @"
                            <w:p>
                                <w:unknown w:val=""unknown tag value"">test</w:unknown>
                            </w:p>
";
            var actual = TestHelper.ParseString(input);

            Assert.IsNotNull(actual);
            Assert.AreEqual("unknown tag value", actual?
                .Children.First()
                    .GetAttribute("value"));
        }
    }
}
