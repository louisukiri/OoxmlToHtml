using System.Collections.Generic;
using System.Data.Common;
using System.Xml;
using OoxmlToHtml.Abstracts;

namespace OoxmlToHtml
{

    public class Token
    {
        protected KeywordToken type;
        protected object value;
        protected Source source;
        public int position;
        protected string text;

        public string Type { get; }
        public KeywordToken Keyword => type;
        public string Text => text;
        public string Literal { get; }

        public const string Code = "Code";

        public const string Illegal = "ILLEGAL";
        public const string Eof = "EOF";

        public const char Start = '<';

        public const char End = '>';

        public const char EQ = '=';

        public const string Identifier = "Identifier";

        public const string Paragraph = "Paragraph";

        public const string Run = "Run";
        
        public const string Color = "Color";

        public const string Value = "Value";

        public const string StringLiteral = "StringLiteral";

        public const string ShortEnd = "/>";

        public const string LongEnd = "</";

        public const char Quote = '"';

        public const string PreviousParagraph = "PreviousParagraph";

        public const string Bold = "Bold";

        public const string Italic = "Italic";

        public const string Size = "Size";

        public const string RunProperty = "RunProperty";

        public const string SpaceAttribute = "SpaceAttribute";

        public const string ParagraphStyle = "ParagraphStyle";

        private readonly IDictionary<string, string> _keyWords = new Dictionary<string, string>()
        {
            {"w:r", Run },
            {"w:p", Paragraph },
            {"w:color", Color },
            {"w:val", Value },
            {"/>", ShortEnd},
            {"</", LongEnd },
            {"w:pPr",  PreviousParagraph},
            {"w:b", Bold },
            {"w:i", Italic },
            {"w:sz", Size },
            {"w:rPr", RunProperty },
            {"xml:space", SpaceAttribute },
            {"w:pStyle", ParagraphStyle }
        };

        public Token(Source source)
        {
            this.source = source;
            this.position = source.Position;
            Extract();
        }
        public Token(string type, string literal)
        {
            Type = type;
            Literal = literal;
        }

        protected char CurrentChar => source.CurrentChar;
        protected char NextCharValue
        {
            get
            {
                source.NextChar();
                return CurrentChar;
            }
        }

        protected void NextChar(int offset = 1) => source.NextChar(offset);

        protected char PeekChar => source.PeekChar;

        protected char PeekCharAhead(int ahead) => source.PeekCharAhead(ahead);

        protected virtual void Extract()
        {
            text = CurrentChar.ToString();
            value = null;
            NextChar();
        }
        public string LookupIdent(string ident)
        {
            return _keyWords.ContainsKey(ident) ?
                _keyWords[ident] :
                StringLiteral;
        }
    }
}
