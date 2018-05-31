using System;
using System.Collections.Generic;
using System.Text;

namespace OoxmlToHtml
{
    public abstract class Scanner
    {
        protected Source Source;
        public Tokens CurrentToken { get; private set; }

        protected Scanner(Source source)
        {
            Source = source;
        }

        public Tokens NextToken()
        {
            CurrentToken = ExtractToken();
            return CurrentToken;
        }

        public abstract Tokens ExtractToken();

        public char CurrentChar => Source.CurrentChar;

        public char NextChar => Source.NextChar;
    }
}
