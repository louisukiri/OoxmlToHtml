using NUnit.Framework;
using OoxmlToHtml.test.Helpers;

namespace OoxmlToHtml.test.Parsers
{
    [TestFixture]
    public class CodeStatementParserTest
    {
        [Test]
        public void ShouldAddCodeAsNextChildOfSibling()
        {
            var result = TestHelper.ParseString(@"<w:body>
                                <w:p>
                                    <w:r>
                                        <w:rPr>
                                            <w:i/>
                                        </w:rPr>
                                        <w:t xml:space=""preserve"">The quick ``` brown fox jumped</w:t>
                                    </w:r>
                                    <w:r>
                                        <w:rPr>
                                            <w:i/>
                                        </w:rPr>
                                        <w:t xml:space=""preserve""> over all of it ``` outside of the code</w:t>
                                    </w:r>
                                  </w:p></w:body>");
            var codeNode = result.child.child.child.Next.child.Next;
            Assert.AreEqual(KeywordToken.Code, codeNode.Type);
            //Assert.AreEqual(result.child.GetAttribute("text"), "");
        }
        
    }
}