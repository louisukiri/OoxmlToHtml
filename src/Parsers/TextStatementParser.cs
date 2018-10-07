namespace OoxmlToHtml.Parsers
{
    public class TextStatementParser : ElementNode
    {
        public TextStatementParser(OoxmlNodeTd parser) : base(parser)
        {
        }

        protected override KeywordToken AttributeName => KeywordToken.Text;
    }
}