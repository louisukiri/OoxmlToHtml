using System;
using System.Linq;
using System.Text;
using OoxmlToHtml.Extensions;

namespace OoxmlToHtml
{
    public class WordToken : Token
    {
        public WordToken(Source source) : base(source)
        {
            Extract();
        }

        protected sealed override void Extract()
        {
            StringBuilder stringBuilder = new StringBuilder();
            while (CurrentChar.IsLetter() || isWordChar(CurrentChar))
            {
                stringBuilder.Append(CurrentChar);
                NextChar();
            }

            text = stringBuilder.ToString();
            type = TokenType.RESERVED_WORDS.ContainsKey(text.ToLower())
                ? TokenType.RESERVED_WORDS[text.ToLower()]
                : KeywordToken.StringLiteral;

        }

        // determines if a character is valid in the context of a word
        static bool isWordChar(char wordChar)
        {
            return new char[]
            {
                '.',
                ',',
                '!',
                '(',
                ')',
                ':',
                '-'
            }.Contains(wordChar);
        }
    }
}