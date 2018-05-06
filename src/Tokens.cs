using System;
using System.Collections.Generic;
using System.Text;

namespace OoxmlToHtml
{

    public class Tokens
    {
        public string Type { get; }
        public string Literal { get; }

        public const string Illegal = "ILLEGAL";
        public const string Eof = "EOF";

        public const char Start = '<';

        public const char End = '>';

        public const char EQ = '=';

        public const string Identifier = "Identifier";

        public const string Paragraph = "Paragraph";

        public const string Run = "Run";

        public const string Text = "Text";

        public const string Color = "Color";

        public const string Value = "Value";

        public const string StringLiteral = "StringLiteral";

        public const string ShortEnd = "/>";

        public const string LongEnd = "</";

        public const char Quote = '"';

        private readonly IDictionary<string, string> _keyWords = new Dictionary<string, string>()
        {
            {"w:r", Run },
            {"w:t", Text },
            {"w:p", Paragraph },
            {"w:color", Color },
            {"w:val", Value },
            {"/>", ShortEnd},
            {"</", LongEnd }
        };


        //public const char WordClass = 'w';
        //public const string Paragraph = "w:p";
        //public const string Paragraphproperty = "w:pPr";
        //public const string Run = "w:r";
        //public const string Text = "w:t";
        //public const string Runproperty = "w:rPr";
        //public const string Color = "w:color";
        //public const string Value = "w:value";


        public Tokens(string type, string literal)
        {
            Type = type;
            Literal = literal;
        }

        public string LookupIdent(string ident)
        {
            return _keyWords.ContainsKey(ident) ?
                _keyWords[ident] :
                StringLiteral;
        }
    }
}
