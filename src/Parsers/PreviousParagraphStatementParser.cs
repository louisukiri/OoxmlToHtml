namespace OoxmlToHtml.Parsers
{
    public class PreviousParagraphStatementParser : ElementNode
    {
        public PreviousParagraphStatementParser(OoxmlNodeTd parent) : base(parent)
        {
        }

        protected override KeywordToken AttributeName => KeywordToken.PreviousParagraph;
    }
}