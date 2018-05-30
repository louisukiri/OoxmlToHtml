using OoxmlToHtml.Extensions;

namespace OoxmlToHtml.Tokens
{
    public class EofToken : Token
    {
        public EofToken(Source source) : base(source)
        {

        }
        protected override void Extract()
        {
            type = KeywordToken.EOF;
            text = null;
        }
    }
}