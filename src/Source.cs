using System;
using System.IO;

namespace OoxmlToHtml
{
    public class Source
    {
        public static char EOL = '\n';
        public static char EOF = '\0';
        private int _position = 0;
        private int _readPosition = 0;
        private bool skipWhiteSpace = true;
        private readonly string _input;
        private char _ch;

        public Source(string input, bool skipWhiteSpace = true)
        {
            _input = input;
            this.skipWhiteSpace = skipWhiteSpace;
            ReadChar();
        }

        public void ReadChar()
        {
            _ch = _readPosition >= _input.Length ? EOF :
                _input[_readPosition];

            _position = _readPosition;
            _readPosition += 1;

        }

        public int Position => _position;

        public char NextChar
        {
            get
            {
                ++_position;
                return CurrentChar;
            }
        }

        public char PeekChar
        {
            get
            {
                if (_readPosition >= _input.Length) return EOF;
                return _input[_readPosition];
            }
        }
        public char CurrentChar => _input[_position];
    }
}