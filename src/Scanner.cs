using System;
using System.Collections.Generic;
using System.Text;

namespace OoxmlToHtml
{
    public abstract class Scanner
    {
        protected Source source;
        public Tokens CurrentToken { get; private set; }

        public Scanner(Source _source)
        {
            source = _source;
        }

        public Tokens NextToken()
        {
            CurrentToken = ExtractToken();
            return CurrentToken;
        }

        public abstract Tokens ExtractToken();

        public char CurrentChar()
        {
            return source.CurrentChar();
        }

        public char NextChar()
        {
            return source.NextChar();
        }
    }
}
