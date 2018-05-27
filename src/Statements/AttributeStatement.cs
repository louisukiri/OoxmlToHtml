using System;
using OoxmlToHtml.Abstracts.Ast;

namespace OoxmlToHtml.Statements
{
    public class AttributeStatement : IStatement
    {
        public string Value { get; private set; }
        public Tokens Token { get; }
        public string Name { get; }

        public AttributeStatement(Tokens token, string value)
        {
            Value = value;
            Token = token;
            Name = token.Literal;
        }
        public string TokenLiteral()
        {
            return Value;
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
