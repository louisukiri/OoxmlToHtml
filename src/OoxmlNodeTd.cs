using OoxmlToHtml.Abstracts;
using OoxmlToHtml.Factories;
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
            var token = NextToken();
            Root = NodeFactory.CreateRootNode();
            
            // only container elements can be processed here
            switch (token.Keyword)
            {
                case KeywordToken.Paragraph:

                    break;
            }
        }
    }
}