using System;
using System.Collections.Generic;
using System.Text;
using OoxmlToHtml.AST.abstracts;
using OoxmlToHtml.Statements;

namespace OoxmlToHtml
{
    public class Parser
    {
        private readonly Lexer _lexer;
        private Tokens _currentToken;
        private Tokens _peekToken;
        public Parser(Lexer lexer)
        {
            _lexer = lexer;

            NextToken();
            NextToken();
        }

        private void NextToken()
        {
            _currentToken = _peekToken;
            _peekToken = _lexer.NextToken();
        }

        private IStatement ParseStatement()
        {
            switch (_currentToken.Type)
            {
                case Tokens.Color:
                    return ParseColorStatement();
                default:
                    return null;
            }
        }

        private IStatement ParseColorStatement()
        {
            var colorToken = _currentToken;
            if (!ExpectPeek(Tokens.Value))
            {
                return null;
            }

            if (!ExpectPeek(Tokens.EQ.ToString()))
            {
                return null;
            }

            if (!ExpectPeek(Tokens.Quote.ToString()))
            {
                return null;
            }

            if (!ExpectPeek(Tokens.StringLiteral))
            {
                return null;
            }

            var statement = new ColorStatement(colorToken, new StringStatement(_currentToken));

            if (!ExpectPeek(Tokens.Quote.ToString()))
            {
                return null;
            }

            if (!ExpectPeek(Tokens.ShortEnd))
            {
                return null;
            }

            return statement;
        }

        private bool CurTokenIs(string tokenType)
        {
            return _currentToken.Type == tokenType;
        }

        private bool PeekTokenIs(string tokenType)
        {
            return _peekToken.Type == tokenType;
        }

        private bool ExpectPeek(string tokenType)
        {
            if (!PeekTokenIs(tokenType)) return false;
            NextToken();
            return true;
        }

        public Program ParseProgram()
        {
            var program = new Program();
            while (_currentToken.Type != Tokens.Eof)
            {
                var statement = ParseStatement();
                if (statement != null)
                {
                    program.AddStatement(statement);
                }
                NextToken();
            }

            return program;
        }
    }
}
