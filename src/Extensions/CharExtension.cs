namespace OoxmlToHtml.Extensions
{
    public static class CharExtension
    {
        public static bool IsLetter(this char ch)
        {
            return (('a' <= ch) && (ch <= 'z')) ||
                   (('A' <= ch) && (ch <= 'Z')) ||
                   (('0' <= ch) && (ch <= '9')) ||
                   ch == ':';
        }
        public static bool IsEOF(this char ch) => ch == '\0';
        public static bool IsSpace(this char ch) => ch == ' ' || ch == '\r' || ch == '\n';

        public static bool IsSymbol(this char ch)
        {
            return TokenType.SYMBOLS.ContainsKey(ch.ToString());
        }

        public static KeywordToken ToTokenType(this char ch)
        {
            return TokenType.SYMBOLS[ch.ToString()];
        }
    }
}