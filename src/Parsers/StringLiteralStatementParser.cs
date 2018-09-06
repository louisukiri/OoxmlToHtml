using System;
using System.Text;
using OoxmlToHtml.Abstracts;
using OoxmlToHtml.Factories;

namespace OoxmlToHtml.Parsers
{
    public class StringLiteralStatementParser : OoxmlNodeTd, IStatementParser
    {
        public StringLiteralStatementParser(OoxmlNodeTd parser) : base(parser)
        {
        }
        
        public virtual INode Parse(Token token)
        {

            var newNode = NodeFactory.CreateNode(KeywordToken.StringLiteral);
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(token.Text);
            stringBuilder.Append(' ');
            while (parser.PeekToken.Keyword == KeywordToken.StringLiteral)
            {
                parser.NextToken();
                stringBuilder.Append(parser.CurrentToken.Text);
                stringBuilder.Append(' ');
            }

            string value = stringBuilder.ToString().TrimEnd();
            newNode.SetAttribute("Text", value);
            // we have consumed all string literals put the cursor on what comes next
            parser.NextToken();
            return newNode;
        }
        
    }
}