using System;
using OoxmlToHtml.Abstracts.Ast;

namespace OoxmlToHtml
{
    public class IdentifierExpression : IExpression
    {
        private Token _token;
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
