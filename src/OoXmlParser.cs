using System;
using System.Collections;
using System.Collections.Generic;
using OoxmlToHtml.Abstracts;
using OoxmlToHtml.Abstracts.Ast;
using OoxmlToHtml.Statements;
using OoxmlToHtml.Visitors;

namespace OoxmlToHtml
{
    public class OoXmlParser : IParser
    {
        private readonly Lexer _lexer;
        private Token _currentToken;
        private Token _peekToken;
        private IList<IAnalyzer> _analyzers = new List<IAnalyzer>();
        private readonly HtmlVisitor _visitor = new HtmlVisitor();
        protected static ISymTab symTab = null;
        protected Scanner scanner;
        protected ICode iCode;

        protected OoXmlParser(Scanner scanner)
        {
            this.scanner = scanner;
        }
        public OoXmlParser(string input) : this(new Lexer(input))
        {

        }
        public OoXmlParser(Lexer lexer)
        {
            _lexer = lexer;

            NextToken();
            NextToken();
        }

        public Token NextToken()
        {
            return scanner.NextToken();
        }
        
        private IStatement ParseStatement()
        {
            switch (_currentToken.Type)
            {
                case Token.Color:
                    return ParseColorStatement();
                case Token.Paragraph:
                    return ParseParagraphStatement();
                case Token.PreviousParagraph:
                    return ParseParagraphPropertyStatement();
                case Token.Run:
                    return ParseRunStatement();
                case Token.Size:
                    return ParseSize();
                case Token.Italic:
                case Token.Bold:
                    return ParseFlag();
                case Token.Value:
                case Token.SpaceAttribute:
                    return ParseAttribute();
                case Token.ParagraphStyle:
                    return ParsePStyle();
                default:
                    return null;
            }
        }

        private IStatement ParsePStyle()
        {
            var statement = new ParagraphStyleStatement(_currentToken);
            while (_currentToken.Type != Token.ShortEnd)
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

            MoveToNext(Token.EQ.ToString());

            if (!ExpectPeek(Token.EQ.ToString()))
            {
                return null;
            }

            if (!ExpectPeek(Token.Quote.ToString()))
            {
                return null;
            }

            if (!ExpectPeek(Token.StringLiteral))
            {
                return null;
            }

            sizeValue = new StringStatement(_currentToken);

            if (!ExpectPeek(Token.Quote.ToString()))
            {
                return null;
            }
            return sizeValue?.Value;
        }
        private IStatement ParseSize()
        {

            var sizeToken = _currentToken;
            var sizeValue = ReadValue(Token.Size);

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
            MoveToNext(Token.End.ToString());
            MoveToNext(Token.StringLiteral);
            NextToken();
            var newStatement = new StringStatement(_currentToken);
            NextToken();
            while (_currentToken.Type == Token.StringLiteral)
            {
                newStatement.AppendValue(_currentToken.Literal);
                NextToken();
            }

            return newStatement;
        }

        private IStatement ParseRunStatement()
        {
            var newstatement = new RunStatement(_currentToken);
            MoveToNext(Token.End.ToString());
            NextToken();
            NextToken();

            while (!(_currentToken.Type == Token.LongEnd && _currentToken.Literal == "w:r"))
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
            MoveToNext(Token.End.ToString());
            NextToken();
            return newStatement;
        }

        private void MoveToNext(string token)
        {
            while (_peekToken.Type != token && _currentToken.Type != Token.Eof)
            {
                NextToken();
            }
        }
        private IStatement ParseParagraphStatement()
        {
            var newStatement = new ParagraphStatement(_currentToken);
            MoveToNext(Token.End.ToString());
            NextToken();
            while (!(_currentToken.Type == Token.LongEnd && _currentToken.Literal == "w:p"))
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
            var colorValue = ReadValue(Token.Color);

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
            while (_currentToken.Type != Token.Eof)
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
                // program = analyzer.Analyze(program);
            }

            return program;
        }

        public Token CurrentToken => scanner.CurrentToken();
        public virtual IParseResult Parse()
        {
            var program = Analyze(ParseProgram());
            _visitor.Visit(program);

            return _visitor;
        }
    }
}
