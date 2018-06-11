namespace OoxmlToHtml.Parsers
{
    public class ParagraphStyleStatementParser: ElementNode
    {
        public ParagraphStyleStatementParser(OoxmlNodeTd parent) : base(parent)
        {
        }

        protected override KeywordToken AttributeName => KeywordToken.ParagraphStyle;
    }
}