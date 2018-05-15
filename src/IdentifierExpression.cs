using System;
using System.Collections.Generic;
using System.Text;
using OoxmlToHtml.AST.abstracts;

namespace OoxmlToHtml
{
    public class IdentifierExpression : IExpression
    {
        private Tokens token;
        public string value { get; private set; }
        public string TokenLiteral()
        {
            throw new NotImplementedException();
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void ExpressionNode()
        {
            throw new NotImplementedException();
        }
    }
}
