namespace OoxmlToHtml.Parsers
{
    public class UnknownStringLiteralAttribute : AttributeNode
    {
        public UnknownStringLiteralAttribute(OoxmlNodeTd parent) : base(parent)
        {
        }

        public override string AttributeName => "unknown";

        public override KeywordToken Token => KeywordToken.StringLiteral
        
        ;
    }
}