using System;
using OoxmlToHtml.Abstracts.Ast;

namespace OoxmlToHtml.Statements
{
    public class StringStatement : IStatement
    {
        public Token Token { get; private set; }
        public string Value = String.Empty;
        public StringStatement(Token token)
        {
            Token = token;
            Value = Token.Literal;
        }
        public string TokenLiteral()
        {
            return Value;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void AppendValue(string value)
        {
            Value += ((Value != string.Empty) ? " " : "") + value;
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
