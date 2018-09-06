using OoxmlToHtml.Abstracts;
using OoxmlToHtml.Factories;

namespace OoxmlToHtml.Parsers
{
    public class CodeParser : OoxmlNodeTd, IStatementParser
    {
        public CodeParser(OoxmlNodeTd parser) : base(parser)
        {
        }
        public INode Parse(Token token)
        {
            parser.NextToken();
            return NodeFactory.CreateNode(KeywordToken.Code);
        }

    }
}