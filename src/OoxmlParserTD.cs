using OoxmlToHtml.Abstracts;
using OoxmlToHtml.Tokens;

namespace OoxmlToHtml
{
    public class OoxmlParserTd : Parser
    {
        public OoxmlParserTd(Scanner scanner) : base(scanner)
        {
        }

        public override void Parse()
        {
            Token token = null;

            while(!(token is EofToken))
            {
                token = NextToken();
            }
        }
    }
}