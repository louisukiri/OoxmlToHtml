namespace OoxmlToHtml.Parsers
{
    public class UnknownElementParser : ElementNode
    {
        public UnknownElementParser(OoxmlNodeTd parent) : base(parent)
        {
        }

        protected override KeywordToken AttributeName => KeywordToken.Unknown;
    }
}