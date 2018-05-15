using System;
using System.Collections.Generic;
using System.Text;
using OoxmlToHtml.AST.abstracts;

namespace OoxmlToHtml.Statements
{
    public class SizeStatement : IStatement
    {
        private string literal;
        public SizeStatement(Tokens token, string size)
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

        public Tokens Token { get; }
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
