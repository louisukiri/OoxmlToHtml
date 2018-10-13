using OoxmlToHtml.Abstracts;

namespace OoxmlToHtml.Parsers
{
    public class ParagraphStatementParser : ElementNode
    {
        public ParagraphStatementParser(OoxmlNodeTd parent) : base(parent)
        {
        }

        protected override KeywordToken AttributeName => KeywordToken.Paragraph;
        public override INode Parse(Token token, int level = 0)
        {
            var node = base.Parse(token, level);
            //parser.NextToken();
            return node;
        }
    }
}