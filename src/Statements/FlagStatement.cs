using System;
using OoxmlToHtml.Abstracts.Ast;

namespace OoxmlToHtml.Statements
{
    public class FlagStatement : IStatement
    {
        public Tokens Token { get; }

        public FlagStatement(Tokens token)
        {
            Token = token;
        }
        public void Accept(IVisitor visitor)
        {
            throw new NotImplementedException();
        }

        public void AddStatement(IStatement childStatement)
        {
            throw new NotImplementedException();
        }

        public void StatementNode()
        {
            throw new NotImplementedException();
        }

        public string TokenLiteral()
        {
            throw new NotImplementedException();
        }
    }
}
