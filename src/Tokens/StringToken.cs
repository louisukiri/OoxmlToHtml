using System.Text;
using OoxmlToHtml.Extensions;

namespace OoxmlToHtml.Tokens
{
    public class StringToken : Token
    {
        public StringToken(Source source) : base(source)
        {

        }

        protected override void Extract()
        {
            var stringBuilder = new StringBuilder();
            NextChar();
            while (CurrentChar != '"')
            {
                stringBuilder.Append(CurrentChar);
                NextChar();
            }
            NextChar();
            text = stringBuilder.ToString();
            type = KeywordToken.StringValue;
        }
    }
}