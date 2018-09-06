using OoxmlToHtml.Abstracts;

namespace OoxmlToHtml.Parsers
{
    public class ParagraphStatementParser : ElementNode
    {
        public ParagraphStatementParser(OoxmlNodeTd parser) : base(parser)
        {
        }

        protected override KeywordToken AttributeName => KeywordToken.Paragraph;
    }
}