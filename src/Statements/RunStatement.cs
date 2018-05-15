using System;
using System.Collections.Generic;
using System.Text;
using OoxmlToHtml.AST.abstracts;

namespace OoxmlToHtml.Statements
{
    public class RunStatement : IStatement
    {
        public Tokens Token { get; }
        public string Text { get; private set; }
        public string TokenLiteral()
        {
            throw new NotImplementedException();
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void StatementNode()
        {
            throw new NotImplementedException();
        }

        public void AddStatement(IStatement childStatement)
        {
            if (childStatement.Token.Type == Tokens.StringLiteral)
            {
                Text = childStatement.TokenLiteral();
            }
        }

        public RunStatement(Tokens token)
        {
            Token = token;
        }
    }
}
