namespace OoxmlToHtml.Parsers
{
    public class ValueAttribute : AttributeNode
    {
        public ValueAttribute(OoxmlNodeTd parent) : base(parent)
        {
        }

        public override string AttributeName => "value";
        public override KeywordToken Token => KeywordToken.Value;
    }
}