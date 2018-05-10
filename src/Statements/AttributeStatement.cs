using System;
using System.Collections.Generic;
using System.Text;
using OoxmlToHtml.AST.abstracts;

namespace OoxmlToHtml
{
    public class AttributeStatement : IStatement
    {
        public IExpression Value { get; private set; }
        public Tokens Token { get; }

        public AttributeStatement(string token, string name, IExpression value)
        {
            Value = value;
        }
        public string TokenLiteral()
        {
            throw new NotImplementedException();
        }


        public void StatementNode()
        {
            throw new NotImplementedException();
        }

        public void AddStatement(IStatement childStatement)
        {
            throw new NotImplementedException();
        }
    }
}
