﻿using System;
using System.Collections.Generic;
using System.Text;

namespace OoxmlToHtml
{

    public class Tokens
    {
        public string Type { get; }
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

        public const string Text = "Text";

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

        private readonly IDictionary<string, string> _keyWords = new Dictionary<string, string>()
        {
            {"w:r", Run },
            {"w:t", Text },
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
            {"xml:space", SpaceAttribute }
        };

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
