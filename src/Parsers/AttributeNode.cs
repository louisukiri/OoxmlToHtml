using OoxmlToHtml.Abstracts;
using OoxmlToHtml.Factories;

namespace OoxmlToHtml.Parsers
{
    public abstract class AttributeNode : OoxmlNodeTd
    {
        protected AttributeNode(OoxmlNodeTd parent) : base(parent)
        {
        }
        protected abstract KeywordToken AttributeName { get; }
        public virtual INode Parse(Token token)
        {
            if (token.Keyword != AttributeName)
            {
                return null;
            }

            token = NextToken();
            if (token.Keyword != KeywordToken.EQ)
            {
                return null;
            }

            token = NextToken();
            if (token.Keyword != KeywordToken.StringValue)
            {
                return null;
            }

            var newNode = NodeFactory.CreateNode(AttributeName);
            newNode.SetAttribute("value", token.Literal);
            return newNode;
        }
    }
}