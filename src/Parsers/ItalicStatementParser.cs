using OoxmlToHtml.Abstracts;

namespace OoxmlToHtml.Parsers
{
    public class ItalicStatementParser : ElementNode
    {
        public ItalicStatementParser(OoxmlNodeTd parent) : base(parent)
        {
        }

        protected override KeywordToken AttributeName => KeywordToken.Italic;
    }
}