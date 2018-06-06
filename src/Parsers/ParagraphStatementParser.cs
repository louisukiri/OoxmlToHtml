using OoxmlToHtml.Abstracts;

namespace OoxmlToHtml.Parsers
{
    public class ParagraphStatementParser : ElementNode
    {
        public ParagraphStatementParser(OoxmlNodeTd parent) : base(parent)
        {
        }

        protected override KeywordToken AttributeName => KeywordToken.Paragraph;
        protected override KeywordToken[] IgnoredTokens => new KeywordToken[]
        {
            KeywordToken.ParagraphStyle,
            KeywordToken.Run,
            KeywordToken.PreviousParagraph,
            KeywordToken.Color
        };
    }
}