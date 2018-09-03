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
                    while (CurrentChar != '>')
                    {
                        stringBuilder.Append(CurrentChar);
                        NextChar();
                    }

                    // NextChar();
                    text = stringBuilder.ToString();
                    // force file ending when w:document closes
                    type = text == "w:document"? KeywordToken.EOF : KeywordToken.Close;
                }
                else if (CurrentChar == '`' && PeekChar == '`' && PeekCharAhead(2) == '`')
                {
                    NextChar(2);                    
                    type = KeywordToken.Code;
                    text = string.Empty;
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