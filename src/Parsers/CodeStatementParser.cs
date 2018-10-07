using OoxmlToHtml.Abstracts;
using OoxmlToHtml.Factories;

namespace OoxmlToHtml.Parsers
{
    public class CodeStatementParser : OoxmlNodeTd, IStatementParser
    {
        public CodeStatementParser(OoxmlNodeTd parent) : base(parent)
        {
        }

        public INode Parse(Token token)
        {
            parser.NextToken();
            return NodeFactory.CreateNode(KeywordToken.Code);
        }
    }
}