using OoxmlToHtml.Abstracts;

namespace OoxmlToHtml.Parsers
{
    public class ItalicStatementParser : ElementNode
    {
        public ItalicStatementParser(OoxmlNodeTd parent) : base(parent)
        {
        }

        protected override KeywordToken AttributeName => KeywordToken.Italic;
        //public override INode Parse(Token token)
        //{
        //    var node = base.Parse(token);
        //    node.SetAttribute("italic", bool.TrueString);
        //    node.SetAttribute(Configuration.AttributeElementPropertyName, bool.TrueString);
        //    return node;
        //}
    }
}