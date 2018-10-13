using System.Text;
using OoxmlToHtml.Abstracts;
using OoxmlToHtml.Factories;

namespace OoxmlToHtml.Parsers
{
    public abstract class AttributeNode : OoxmlNodeTd, IAttributeStatementParser
    {
        protected AttributeNode(OoxmlNodeTd parent) : base(parent)
        {
        }

        public abstract string AttributeName { get; }
        public abstract KeywordToken Token { get; }
        
        public virtual INode Parse(Token token, int level = 0)
        {
            if (token.Keyword != Token)
            {
                return null;
            }

            NextToken();
            if (CurrentToken.Keyword != KeywordToken.EQ)
            {
                return null;
            }

            NextToken();
            var newNode = NodeFactory.CreateNode(Token);
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