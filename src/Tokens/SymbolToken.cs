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
                    NextChar(2);
                    var stringBuilder = new StringBuilder();
                    while (CurrentChar.IsLetter())
                    {
                        stringBuilder.Append(CurrentChar);
                        NextChar();
                    }
                    type = KeywordToken.Close;
                    text = stringBuilder.ToString();
                }
                else if (CurrentChar == '`' && PeekChar == '`' && PeekCharAhead(2) == '`')
                {
                    NextChar(3);
                    var stringBuilder = new StringBuilder();
                    while (!(CurrentChar == '`' && PeekChar == '`' && PeekCharAhead(2) == '`'))
                    {
                        stringBuilder.Append(CurrentChar);
                        NextChar();
                    }
                    NextChar(2);
                    type = KeywordToken.Code;
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