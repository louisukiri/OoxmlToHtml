using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OoxmlToHtml
{
    public class Lexer
    {
        private readonly string _input;
        private int _position = 0;
        private int _readPosition = 0;
        private char _ch;

        public Lexer(string input)
        {
            this._input = input;
            ReadChar();
        }

        public Tokens NextToken()
        {
            SkipWhitespace();
            Tokens tok = new Tokens("","");
            if (_ch == '\0')
            {
                return new Tokens(Tokens.Eof, "EOF");
            }
            switch (_ch)
            {
                case '<':
                    if (PeekNext('/'))

                    {
                        ReadChar();
                        ReadChar();
                        tok = new Tokens(Tokens.LongEnd, ReadIdentifier());
                    }
                    else
                    {
                        tok = new Tokens(Tokens.Start.ToString(), _ch.ToString());
                    }
                    break;
                case '>':
                    tok = new Tokens(Tokens.End.ToString(), _ch.ToString());
                    break;
                case '"':
                    tok = new Tokens(Tokens.Quote.ToString(), _ch.ToString());
                    break;
                case '=':
                    tok = new Tokens(Tokens.EQ.ToString(), _ch.ToString());
                    break;
                case '/':
                    if (PeekNext('>'))
                    {
                        ReadChar();
                        tok = new Tokens(Tokens.ShortEnd, "/>");
                    }
                    else
                    {
                        tok = new Tokens(Tokens.Illegal, "\\");
                    }
                    break;
                default:
                    if (IsLetter(_ch))
                    {
                        var literal = ReadIdentifier();
                        tok = new Tokens(tok.LookupIdent(literal),
                            literal);
                        return tok;
                    }
                    else if (IsSpecialChar())
                    {
                        tok = new Tokens(Tokens.Code, "```");
                        return tok;
                    }
                    break;
            }
            ReadChar();
            return tok;
        }

        private bool IsSpecialChar()
        {
            ReadChar();
            if ('`' != _ch)
            {
                return false;
            }
            ReadChar();
            if ('`' != _ch)
            {
                return false;
            }
            ReadChar();
            return true;
        }

        private bool PeekNext(char expected)
        {
            return _input[_readPosition] == expected;
        }
        private void SkipWhitespace()
        {
            char[] whiteSpaceChars = {
                '\r',
                '\n',
                ' '
            };
            while (Array.Exists(whiteSpaceChars, z => z == _ch))
            {
                ReadChar();
            }
        }

        private bool IsLetter(char ch)
        {
            return (('a' <= ch) && (ch <= 'z')) ||
                (('A' <= ch) && (ch <= 'Z')) ||
                   (('0' <= ch) && (ch <= '9')) ||
                ch == ':';
        }

        private bool IsSpace(char ch) => ch == ' ' || ch == '\r' || ch == '\n';

        private string ReadIdentifier(bool includeSpace = false)
        {
            var oldPosition = _position;
            while (IsLetter(_ch) || (includeSpace && IsSpace(_ch)))
            {
                ReadChar();
            }

            return _input.Substring(oldPosition, _position - oldPosition);
        }

        private string LookupIdent(string ident)
        {
            return "";
        }
        public void ReadChar()
        {
            _ch = _readPosition >= _input.Length ? '\0' :
                _input[_readPosition];

            _position = _readPosition;
            _readPosition += 1;

        }
    }
}
