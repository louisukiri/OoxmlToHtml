namespace OoxmlToHtml.Parsers
{
    public class ValueStatementNode : AttributeNode
    {
        public ValueStatementNode(OoxmlNodeTd parent) : base(parent)
        {
        }

        protected override KeywordToken AttributeName => KeywordToken.Value;
    }
}