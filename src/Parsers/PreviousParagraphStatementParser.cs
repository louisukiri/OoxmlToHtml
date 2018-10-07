using OoxmlToHtml.Abstracts;

namespace OoxmlToHtml.Parsers
{
    public class PreviousParagraphStatementParser : ElementNode
    {
        public PreviousParagraphStatementParser(OoxmlNodeTd parent) : base(parent)
        {
        }

        protected override KeywordToken AttributeName => KeywordToken.PreviousParagraph;
        public override INode Parse(Token token)
        {
            var node = base.Parse(token);
            parser.NextToken();
            return node;
        }
    }
}