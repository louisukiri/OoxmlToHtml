namespace OoxmlToHtml.Parsers
{
    public class PreviousParagraphStatementParser : ElementNode
    {
        public PreviousParagraphStatementParser(OoxmlNodeTd parser) : base(parser)
        {
        }

        protected override KeywordToken AttributeName => KeywordToken.PreviousParagraph;
    }
}