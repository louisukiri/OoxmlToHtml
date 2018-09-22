using OoxmlToHtml.Extensions;

namespace OoxmlToHtml.Tokens
{
    public class EofToken : Token
    {
        public EofToken(Source source) : base(source)
        {
            Extract();
        }
        protected sealed override void Extract()
        {
            type = KeywordToken.EOF;
            text = null;
        }
    }
}