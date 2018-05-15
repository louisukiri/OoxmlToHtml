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
                case Tokens.Paragraph:
                    return ParseParagraphStatement();
                case Tokens.PreviousParagraph:
                    return ParseParagraphPropertyStatement();
                case Tokens.Run:
                    return ParseRunStatement();
                case Tokens.Text:
                    return ParseText();
                case Tokens.Size:
                    return ParseSize();
                case Tokens.Italic:
                case Tokens.Bold:
                    return ParseFlag();
                default:
                    return null;
            }
        }

        private string ReadValue()
        {
            var sizeToken = _currentToken;

            StringStatement sizeValue = null;
            while (_currentToken.Type != Tokens.ShortEnd)
            {
                if (_currentToken.Type == Tokens.Value)
                {
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

                    sizeValue = new StringStatement(_currentToken);

                    if (!ExpectPeek(Tokens.Quote.ToString()))
                    {
                        return null;
                    }
                }
                NextToken();
            }

            return sizeValue?.Value;
        }
        private IStatement ParseSize()
        {

            var sizeToken = _currentToken;
            var sizeValue = ReadValue();

            if (sizeValue == null) return null;

            return new SizeStatement(sizeToken, sizeValue);
        }

        private IStatement ParseFlag()
        {
            var newStatement = new FlagStatement(_currentToken);
            NextToken();
            return newStatement;
        }

        private IStatement ParseText()
        {
            MoveToNext(Tokens.StringLiteral);
            NextToken();
            var newStatement = new StringStatement(_currentToken);
            NextToken();
            while (_currentToken.Type == Tokens.StringLiteral)
            {
                newStatement.AppendValue(_currentToken.Literal);
                NextToken();
            }

            return newStatement;
        }

        private IStatement ParseRunStatement()
        {
            var newstatement = new RunStatement(_currentToken);
            MoveToNext(Tokens.End.ToString());
            NextToken();

            while (!(_currentToken.Type == Tokens.LongEnd && _currentToken.Literal == "w:r"))
            {
                var statement = ParseStatement();
                if (statement != null)
                {
                    newstatement.AddStatement(statement);
                }
                NextToken();
            }
            return newstatement;
        }
        private IStatement ParseParagraphPropertyStatement()
        {
            var newStatement = new ParagraphPropertyStatement(_currentToken);
            MoveToNext(Tokens.End.ToString());
            NextToken();
            return newStatement;
        }

        private void MoveToNext(string token)
        {
            while (_peekToken.Type != token && _currentToken.Type != Tokens.Eof)
            {
                NextToken();
            }
        }
        private IStatement ParseParagraphStatement()
        {
            if (!ExpectPeek(Tokens.End.ToString()))
            {
                return null;
            }

            var newStatement = new ParagraphStatement(_currentToken);
            while (!(_currentToken.Type == Tokens.LongEnd && _currentToken.Literal == "w:p"))
            {
                var statement = ParseStatement();
                if (statement != null)
                {
                    newStatement.AddStatement(statement);
                }
                NextToken();
            }
            return newStatement;
        }
        private IStatement ParseColorStatement()
        {
            var colorToken = _currentToken;
            var colorValue = ReadValue();

            if (colorValue == null)
                return null;

            return new ColorStatement(colorToken, colorValue);
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
