using System;
using OoxmlToHtml.Abstracts.Ast;

namespace OoxmlToHtml.Statements
{
    public class ColorStatement : IStatement
    {
        public Token Token { get; private set; }
        public string Value { get; private set; }
        
        public ColorStatement(Token token, string value)
        {
            Token = token;
            Value = value;
        }

        public string TokenLiteral()
        {
            return Value;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void StatementNode()
        {
        }

        public void AddStatement(IStatement childStatement)
        {
            throw new NotImplementedException();
        }
    }
}
