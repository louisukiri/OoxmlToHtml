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
            StringBuilder stringBuilder = new StringBuilder();

            while (CurrentToken.Keyword != KeywordToken.EOF
                   && CurrentToken.Keyword == KeywordToken.StringLiteral)
            {
                stringBuilder.Append(CurrentToken.Text);
                stringBuilder.Append(' ');
                NextToken();
            }

            var newNode = NodeFactory.CreateNode(KeywordToken.StringLiteral);
            newNode.SetAttribute("Text", stringBuilder.ToString().TrimEnd());
            return newNode;
        }
        
    }
}