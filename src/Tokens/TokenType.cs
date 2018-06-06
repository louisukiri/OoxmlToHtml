using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OoxmlToHtml.Abstracts;
using OoxmlToHtml.Extensions;

namespace OoxmlToHtml
{
    public class TokenAttribute : Attribute
    {
        public string Text;
        public bool IsChar = false;
        internal TokenAttribute(string text)
        {
            Text = text;
        }

        internal TokenAttribute(char ch)
        {
            Text = ch.ToString();
            IsChar = true;
        }
    }

    public enum KeywordToken
    {
        [TokenAttribute("Identifier")] Identifier,
        [TokenAttribute("StringLiteral")] StringLiteral,
        [TokenAttribute("w:r")] Run,
        [TokenAttribute("w:t")] Text,
        [TokenAttribute("w:p")] Paragraph,
        [TokenAttribute("w:color")] Color,
        [TokenAttribute("w:pPr")] PreviousParagraph,
        [TokenAttribute("w:b")] Bold,
        [TokenAttribute("w:i")] Italic,
        [TokenAttribute("w:sz")] Size,
        [TokenAttribute("w:val")] Value,
        [TokenAttribute("StringValue")] StringValue,
        [TokenAttribute("/>")] ShortClose,
        [TokenAttribute("</")] Close,
        [TokenAttribute("xml:space")] Space,
        [TokenAttribute("w:pStyle")] ParagraphStyle,
        // ReSharper disable InconsistentNaming
        [TokenAttribute('<')] STARTING_ELEMENT,
        [TokenAttribute('>')] ENDING_ELEMENT,
        [TokenAttribute(':')] NAMESPACE_DELIM,
        [TokenAttribute('/')] SLASH,
        [TokenAttribute('=')] EQ,
        [TokenAttribute("EOF")] EOF,
        [TokenAttribute('`')] Code,
        [TokenAttribute("")] Unknown
        // ReSharper restore InconsistentNaming
    }
    
    public class TokenType : ITokenType
    {
        public TokenType()
        {
        }

        private static Dictionary<string, KeywordToken> _reservedWords;

        public static Dictionary<string, KeywordToken> RESERVED_WORDS
        {
            get
            {
                if (_reservedWords != null)
                    return _reservedWords;

                _reservedWords = new Dictionary<string, KeywordToken>();
                foreach (var z in Enum.GetNames(typeof(KeywordToken)))
                {
                    var e = (KeywordToken)Enum.Parse(typeof(KeywordToken), z);
                    if (e.IsCharTokenType())
                    {
                        continue;
                    }
                    _reservedWords.Add(e.GetText().ToLower(), (KeywordToken)Enum.Parse(typeof(KeywordToken), z));
                }

                return _reservedWords;
            }
        }
        private static Dictionary<string, KeywordToken> _symbols;

        public static Dictionary<string, KeywordToken> SYMBOLS
        {
            get
            {
                if (_symbols != null)
                    return _symbols;

                _symbols = new Dictionary<string, KeywordToken>();
                foreach (var z in Enum.GetNames(typeof(KeywordToken)))
                {
                    var e = (KeywordToken)Enum.Parse(typeof(KeywordToken), z);
                    if (!e.IsCharTokenType())
                    {
                        continue;
                    }
                    _symbols.Add(e.GetText(), (KeywordToken)Enum.Parse(typeof(KeywordToken), z));
                }

                return _symbols;
            }
        }
    }
}