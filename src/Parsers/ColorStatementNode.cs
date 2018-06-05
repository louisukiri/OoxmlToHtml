namespace OoxmlToHtml.Parsers
{
    public class ColorStatementNode : ElementNode
    {
        public ColorStatementNode(OoxmlNodeTd parent) : base(parent)
        {
        }

        protected override KeywordToken AttributeName => KeywordToken.Color;
    }
}