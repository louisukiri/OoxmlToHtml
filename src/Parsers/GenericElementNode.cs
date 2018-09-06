namespace OoxmlToHtml.Parsers
{
    public class GenericElementNode : ElementNode
    {
        public GenericElementNode(OoxmlNodeTd parser, KeywordToken token) : base(parser)
        {
            AttributeName = token;
        }

        protected override KeywordToken AttributeName { get; }
    }
}