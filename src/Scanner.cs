using System;
using System.Collections.Generic;
using System.Text;

namespace OoxmlToHtml
{
    public abstract class Scanner
    {
        protected Source Source;
        private Token _currentToken;
        protected char EOF => Source.EOF;
        protected Scanner(Source source)
        {
            Source = source;
        }

        public Token PeekToken()
        {
            var currentPosition = Source.Position;
            var result = NextToken();
            Source.NextChar(currentPosition - Source.Position);
            return result;
        }

        public Token NextToken()
        {
            _currentToken = ExtractToken();
            return CurrentToken();
        }

        public Token CurrentToken()
        {
            return _currentToken;
        }

        public abstract Token ExtractToken();

        public char CurrentChar => Source.CurrentChar;

        public char NextChar
        {
            get
            {
                Source.NextChar();
                return CurrentChar;
            }
        }
    }
}
