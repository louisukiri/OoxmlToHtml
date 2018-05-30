namespace OoxmlToHtml.Extensions
{
    public static class StringExtension
    {
        public static KeywordToken ToTokenType(this string str)
        {
            return TokenType.RESERVED_WORDS[str];
        }
    }
}