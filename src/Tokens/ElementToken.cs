using System.Text;
using OoxmlToHtml.Extensions;

namespace OoxmlToHtml.Tokens
{
    public class ElementToken : Token
    {
        public ElementToken(Source source) : base(source)
        {
        }
        
        protected override void Extract()
        {
            StringBuilder stringBuilder = new StringBuilder();
            while (CurrentChar.IsLetter())
            {
                stringBuilder.Append(CurrentChar);
                NextChar();
            }

            text = stringBuilder.ToString();
            type = TokenType.RESERVED_WORDS.ContainsKey(text.ToLower())
                ? TokenType.RESERVED_WORDS[text.ToLower()]
                : KeywordToken.Unknown;

        }

    }
}