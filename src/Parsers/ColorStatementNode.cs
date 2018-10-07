using OoxmlToHtml.Abstracts;

namespace OoxmlToHtml.Parsers
{
    public class ColorStatementNode : ElementNode
    {
        public ColorStatementNode(OoxmlNodeTd parser) : base(parser)
        {
        }

        protected override KeywordToken AttributeName => KeywordToken.Color;
        //public override INode Parse(Token token)
        //{
        //    var node = base.Parse(token);
        //    node.SetAttribute("fontColor", node.GetAttribute("value"));
        //    node.SetAttribute(Configuration.AttributeElementPropertyName, bool.TrueString);
        //    node.RemoveAttribute("value");
        //    return node;
        //}
    }
}