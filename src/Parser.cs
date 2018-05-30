using System;
using System.Collections;
using System.Collections.Generic;
using OoxmlToHtml.Abstracts;
using OoxmlToHtml.Abstracts.Ast;
using OoxmlToHtml.Statements;
using OoxmlToHtml.Visitors;

namespace OoxmlToHtml
{
    public class Parser : IParser
    {
        private readonly Lexer _lexer;
        private Tokens _currentToken;
        private Tokens _peekToken;
        private IList<IAnalyzer> _analyzers = new List<IAnalyzer>();
        private readonly HtmlVisitor _visitor = new HtmlVisitor();

        public Parser(string input) : this(new Lexer(input))
        {

        }
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
                case Tokens.Value:
                case Tokens.SpaceAttribute:
                    return ParseAttribute();
                case Tokens.ParagraphStyle:
                    return ParsePStyle();
                default:
                    return null;
            }
        }

        private IStatement ParsePStyle()
        {
            var statement = new ParagraphStyleStatement(_currentToken);
            while (_currentToken.Type != Tokens.ShortEnd)
            {
                NextToken();
                var childStatement = ParseStatement();
                if (childStatement != null)
                {
                    statement.AddStatement(childStatement);
                }
            }

            return statement;
        }

        private string ReadValue(string attributeName)
        {
            var sizeToken = _currentToken;

            StringStatement sizeValue = null;
            if (_currentToken.Type != attributeName)
                return null;

            MoveToNext(Tokens.EQ.ToString());

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
            return sizeValue?.Value;
        }
        private IStatement ParseSize()
        {

            var sizeToken = _currentToken;
            var sizeValue = ReadValue(Tokens.Size);

            if (sizeValue == null) return null;

            return new SizeStatement(sizeToken, sizeValue);
        }

        private IStatement ParseFlag()
        {
            var newStatement = new FlagStatement(_currentToken);
            NextToken();
            return newStatement;
        }

        private IStatement ParseAttribute()
        {
            var attribToken = _currentToken;
            var value = ReadValue(_currentToken.Type);
            var newStatement = new AttributeStatement(attribToken, value);
            return newStatement;
        }

        private IStatement ParseText()
        {
            MoveToNext(Tokens.End.ToString());
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
            var newStatement = new ParagraphStatement(_currentToken);
            MoveToNext(Tokens.End.ToString());
            NextToken();
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
            var colorValue = ReadValue(Tokens.Color);

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

        public IProgram ParseProgram()
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

        public IParser Use(IAnalyzer analyzer)
        {
            _analyzers.Add(analyzer);
            return this;
        }

        public IProgram Analyze(IProgram program)
        {
            foreach (var analyzer in _analyzers)
            {
                program = analyzer.Analyze(program);
            }

            return program;
        }

        public IParseResult Parse()
        {
            var program = Analyze(ParseProgram());
            _visitor.Visit(program);

            return _visitor;
        }
    }
}
