using System;
using System.Linq;
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
                else if (CurrentChar == '/')
                {
                    NextChar();
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
                else
                {
                    type = CurrentChar.ToTokenType();
                    text = CurrentChar.ToString();
                }
            }
            if (type != KeywordToken.EOF)
                NextChar();
        }

        public static Token Create(Source source)
        {
            if (source.CurrentChar != '<'
                || !new[] {'w', '/'}.Contains(source.PeekChar))
            {
                return new SymbolToken(source);
            }
            source.NextChar();
            if (source.CurrentChar == 'w')
            {
                return new ElementToken(source);
            }
            return new SymbolToken(source);
        }
    }
}