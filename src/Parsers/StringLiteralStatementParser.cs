using System.Text;
using OoxmlToHtml.Abstracts;
using OoxmlToHtml.Factories;

namespace OoxmlToHtml.Parsers
{
    public class StringLiteralStatementParser : IStatementParser
    {
        private readonly OoxmlNodeTd _parser;
        public StringLiteralStatementParser(OoxmlNodeTd parent)
        {
            this._parser = parent.parser;
        }
        
        public virtual INode Parse(Token token)
        {
            StringBuilder stringBuilder = new StringBuilder();

            while (_parser.CurrentToken.Keyword != KeywordToken.EOF
                   && _parser.CurrentToken.Keyword == KeywordToken.StringLiteral)
            {
                stringBuilder.Append(_parser.CurrentToken.Text);
                stringBuilder.Append(' ');

                if (_parser.PeekToken().Keyword == KeywordToken.Code)
                {
                    break;
                }
                _parser.NextToken();
            }
            
            var newNode = NodeFactory.CreateNode(KeywordToken.StringLiteral);
            newNode.SetAttribute("Text", stringBuilder.ToString().TrimEnd());
            return newNode;
        }
        
    }
}