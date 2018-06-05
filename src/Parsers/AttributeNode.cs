using System.Text;
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

            NextToken();
            if (CurrentToken.Keyword != KeywordToken.EQ)
            {
                return null;
            }

            NextToken();
            var newNode = NodeFactory.CreateNode(AttributeName);
            if (CurrentToken.Keyword != KeywordToken.StringValue
                && CurrentToken.Keyword != KeywordToken.StringLiteral)
            {
                return null;
            }

            if (CurrentToken.Keyword == KeywordToken.StringValue)
            {
                newNode.SetAttribute("value", CurrentToken.Text);
                return newNode;
            }
            StringBuilder stringBuilder = new StringBuilder();
            while (CurrentToken.Keyword == KeywordToken.StringLiteral)
            {
                stringBuilder.Append(CurrentToken.Text);
            }

            newNode.SetAttribute("value", stringBuilder.ToString());

            NextToken();
            return newNode;
        }
    }
}