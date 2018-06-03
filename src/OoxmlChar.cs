namespace OoxmlToHtml
{
    public struct OoxmlChar
    {
        public readonly char Value;
        public OoxmlChar(char ch)
        {
            Value = ch;
        }
        public static bool IsText()
        {
            return false;
        }
    }
}