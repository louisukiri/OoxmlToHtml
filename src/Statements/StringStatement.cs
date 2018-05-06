using System;
using System.Collections.Generic;
using System.Text;
using OoxmlToHtml.AST.abstracts;

namespace OoxmlToHtml.Statements
{
    public class StringStatement : IStatement
    {
        private readonly Tokens _token;
        public StringStatement(Tokens tokens)
        {
            _token = tokens;
        }
        public string TokenLiteral()
        {
            return _token.Literal;
        }

        public void StatementNode()
        {
        }
    }
}
