namespace OoxmlToHtml.Parsers
{
    public class UnknownStringLiteralAttribute : AttributeNode
    {
        public UnknownStringLiteralAttribute(OoxmlNodeTd parent) : base(parent)
        {
        }

        protected override KeywordToken AttributeName => KeywordToken.StringLiteral;
    }
}