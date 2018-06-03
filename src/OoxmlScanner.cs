using OoxmlToHtml.Extensions;
using OoxmlToHtml.Tokens;

namespace OoxmlToHtml
{
    public class OoxmlScanner : Scanner
    {
        public OoxmlScanner(Source source) : base(source)
        {
        }

        public override Token ExtractToken()
        {
            SkipWhitespace();
            Token token = null;
            char currentChar = CurrentChar;

            if (currentChar.IsEOF())
            {
                token = new EofToken(Source);
            }
            else if (currentChar.IsLetter())
            {
                token = new WordToken(Source);
            }
            else if (currentChar == '"')
            {
                token = new StringToken(Source);
            }
            else if (currentChar.IsSymbol())
            {
                token = new SymbolToken(Source);
            }
            else
            {
                token = new Token(Source);
            }

            return token;
        }
        private void SkipWhitespace()
        {
            char currentChar = CurrentChar;
            while (currentChar.IsSpace())
            {
                currentChar = NextChar;
            }
        }
    }
}