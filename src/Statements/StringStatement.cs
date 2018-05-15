using System;
using System.Collections.Generic;
using System.Text;
using OoxmlToHtml.AST.abstracts;

namespace OoxmlToHtml.Statements
{
    public class StringStatement : IStatement
    {
        public Tokens Token { get; private set; }
        public string Value = String.Empty;
        public StringStatement(Tokens tokens)
        {
            Token = tokens;
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
