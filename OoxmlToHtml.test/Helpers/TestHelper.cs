using OoxmlToHtml.Abstracts;
using OoxmlToHtml.Analyzers;
using OoxmlToHtml.Parsers;
using OoxmlToHtml.Printers;

namespace OoxmlToHtml.test.Helpers
{
    public static class TestHelper
    {
        public static INode ParseString(string text)
        {
            OoxmlNodeTd parent = new OoxmlNodeTd(new OoxmlScanner(new Source(text)));
            //parent.Use(new ElementToAttributeAnalyzer());
            //parent.Use(new AttributeCopierAnalyzer());
            parent.Parse();

            return parent.Root.Root;
        }
    }
}