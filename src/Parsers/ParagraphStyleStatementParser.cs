namespace OoxmlToHtml.Parsers
{
    public class ParagraphStyleStatementParser: ElementNode
    {
        public ParagraphStyleStatementParser(OoxmlNodeTd parser) : base(parser)
        {
        }

        protected override KeywordToken AttributeName => KeywordToken.ParagraphStyle;
    }
}