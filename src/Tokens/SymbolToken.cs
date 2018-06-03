using System.Text;
using OoxmlToHtml.Extensions;

namespace OoxmlToHtml.Tokens
{
    public class SymbolToken : Token
    {
        public SymbolToken(Source source) : base(source)
        {
        }

        protected override void Extract()
        {
            if (CurrentChar.IsSymbol())
            {
                if (CurrentChar == '/' && PeekChar == '>')
                {
                    type = KeywordToken.ShortClose;
                    text = KeywordToken.ShortClose.GetText();
                    NextChar();
                }
                else if (CurrentChar == '<' && PeekChar == '/')
                {
                    NextChar();
                    NextChar();
                    var stringBuilder = new StringBuilder();
                    while (CurrentChar.IsLetter())
                    {
                        stringBuilder.Append(CurrentChar);
                        NextChar();
                    }
                    type = KeywordToken.Close;
                    text = stringBuilder.ToString();
                }
                else
                {
                    type = CurrentChar.ToTokenType();
                    text = CurrentChar.ToString();
                }
            }
            NextChar();
        }
    }
}