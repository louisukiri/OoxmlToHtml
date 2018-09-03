using System;
using System.Text;
using OoxmlToHtml.Abstracts;
using OoxmlToHtml.Factories;

namespace OoxmlToHtml.Parsers
{
    public class StringLiteralStatementParser : OoxmlNodeTd, IStatementParser
    {
        public StringLiteralStatementParser(OoxmlNodeTd parent) : base(parent)
        {
        }
        
        public virtual INode Parse(Token token)
        {

            var newNode = NodeFactory.CreateNode(KeywordToken.StringLiteral);
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(token.Text);
            stringBuilder.Append(' ');
            while (_parent.PeekToken.Keyword == KeywordToken.StringLiteral)
            {
                _parent.NextToken();
                stringBuilder.Append(_parent.CurrentToken.Text);
                stringBuilder.Append(' ');
            }

            string value = stringBuilder.ToString().TrimEnd();
            newNode.SetAttribute("Text", value);
            // we have consumed all string literals put the cursor on what comes next
            _parent.NextToken();
            return newNode;
        }
        
    }
}