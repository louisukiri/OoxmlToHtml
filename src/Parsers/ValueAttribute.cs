namespace OoxmlToHtml.Parsers
{
    public class ValueAttribute : AttributeNode
    {
        public ValueAttribute(OoxmlNodeTd parser) : base(parser)
        {
        }

        public override string AttributeName => "value";
        public override KeywordToken Token => KeywordToken.Value;
    }
}