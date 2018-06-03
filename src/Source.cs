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

        public void NextChar()
        {
            ++_position;
        }

        public char PeekChar
        {
            get
            {
                if (_position + 1 >= _input.Length) return EOF;
                return _input[_position + 1];
            }
        }
        public char CurrentChar
        {
            get
            {
                if (_position >= _input.Length) return EOF;
                return _input[_position];
            }
        }
    }
}