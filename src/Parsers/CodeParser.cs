using OoxmlToHtml.Abstracts;
using OoxmlToHtml.Factories;

namespace OoxmlToHtml.Parsers
{
    public class CodeParser : OoxmlNodeTd, IStatementParser
    {
        public CodeParser(OoxmlNodeTd parent) : base(parent)
        {
        }
        public INode Parse(Token token)
        {
            _parent.NextToken();
            return NodeFactory.CreateNode(KeywordToken.Code);
        }

    }
}