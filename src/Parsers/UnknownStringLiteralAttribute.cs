namespace OoxmlToHtml.Parsers
{
    public class UnknownStringLiteralAttribute : AttributeNode
    {
        public UnknownStringLiteralAttribute(OoxmlNodeTd parser) : base(parser)
        {
        }

        public override string AttributeName => "unknown";

        public override KeywordToken Token => KeywordToken.StringLiteral;
    }
}