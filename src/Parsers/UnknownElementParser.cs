namespace OoxmlToHtml.Parsers
{
    public class UnknownElementParser : ElementNode
    {
        public UnknownElementParser(OoxmlNodeTd parser) : base(parser)
        {
        }

        protected override KeywordToken AttributeName => KeywordToken.Unknown;
    }
}