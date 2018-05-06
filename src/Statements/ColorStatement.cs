using System;
using System.Collections.Generic;
using System.Text;
using OoxmlToHtml.AST.abstracts;

namespace OoxmlToHtml.Statements
{
    public class ColorStatement : IStatement
    {
        public Tokens Token { get; private set; }
        public StringStatement Value { get; private set; }

        public ColorStatement(Tokens token, StringStatement value)
        {
            Token = token;
            Value = value;
        }

        public string TokenLiteral()
        {
            return Token.Literal;
        }

        public void StatementNode()
        {
        }
    }
}
