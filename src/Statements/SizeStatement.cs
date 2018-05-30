using System;
using OoxmlToHtml.Abstracts.Ast;

namespace OoxmlToHtml.Statements
{
    public class SizeStatement : IStatement
    {
        private string literal;
        public SizeStatement(Token token, string size)
        {
            Token = token;
            literal = size;
        }
        public string TokenLiteral()
        {
            return literal;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public Token Token { get; }
        public void StatementNode()
        {
            throw new NotImplementedException();
        }

        public void AddStatement(IStatement childStatement)
        {
            throw new NotImplementedException();
        }
    }
}
