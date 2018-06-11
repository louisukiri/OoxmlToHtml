namespace OoxmlToHtml.Parsers
{
    public class GenericElementNode : ElementNode
    {
        public GenericElementNode(OoxmlNodeTd parent, KeywordToken token) : base(parent)
        {
            AttributeName = token;
        }

        protected override KeywordToken AttributeName { get; }
    }
}