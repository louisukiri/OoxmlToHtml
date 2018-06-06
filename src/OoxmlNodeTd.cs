using OoxmlToHtml.Abstracts;
using OoxmlToHtml.Factories;
using OoxmlToHtml.Parsers;
using OoxmlToHtml.Tokens;

namespace OoxmlToHtml
{
    public class OoxmlNodeTd : Parser
    {
        public IRootNode Root { get; private set; }
        public OoxmlNodeTd(Scanner scanner) : base(scanner)
        {
        }

        public OoxmlNodeTd(OoxmlNodeTd parent) : base(parent.scanner)
        {

        }

        public override void Parse()
        {
            NextToken();
            Root = NodeFactory.CreateRootNode();

            while (CurrentToken.Keyword != KeywordToken.EOF)
            {
                // only container elements can be processed here
                switch (CurrentToken.Keyword)
                {
                    case KeywordToken.Paragraph:
                        var a = new ParagraphStatementParser(this);
                        Root.SetRootNode(a.Parse(CurrentToken));
                        break;
                }

                NextToken();
            }
        }
    }
}