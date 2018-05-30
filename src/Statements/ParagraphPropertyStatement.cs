using System;
using OoxmlToHtml.Abstracts.Ast;

namespace OoxmlToHtml.Statements
{
    public class ParagraphPropertyStatement : IStatement
    {
        public string TokenLiteral()
        {
            throw new NotImplementedException();
        }

        public Token Token { get; }

        public ParagraphPropertyStatement(Token token)
        {
            Token = token;
        }
        public void StatementNode()
        {
            throw new NotImplementedException();
        }

        public void AddStatement(IStatement childStatement)
        {
            throw new NotImplementedException();
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}