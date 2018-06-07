using OoxmlToHtml.Abstracts;
using OoxmlToHtml.Parsers;

namespace OoxmlToHtml.test.Helpers
{
    public static class TestHelper
    {
        public static INode ParseString(string text)
        {
            OoxmlNodeTd parent = new OoxmlNodeTd(new OoxmlScanner(new Source(text)));
            var parser = new ParagraphStatementParser(parent);

            return parser.Parse(parser.CurrentToken);
        }
    }
}