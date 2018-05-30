namespace OoxmlToHtml.Parsers
{
    public class TextStatementParser : ElementNode
    {
        public TextStatementParser(OoxmlNodeTd parent) : base(parent)
        {
        }

        protected override KeywordToken AttributeName => KeywordToken.Text;
    }
}