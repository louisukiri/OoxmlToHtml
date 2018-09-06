using OoxmlToHtml.Abstracts;

namespace OoxmlToHtml.Parsers
{
    public class ItalicStatementParser : ElementNode
    {
        public ItalicStatementParser(OoxmlNodeTd parser) : base(parser)
        {
        }

        protected override KeywordToken AttributeName => KeywordToken.Italic;
    }
}