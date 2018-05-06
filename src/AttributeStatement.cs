using System;
using System.Collections.Generic;
using System.Text;
using OoxmlToHtml.AST.abstracts;

namespace OoxmlToHtml
{
    public class AttributeStatement : IStatement
    {
        private Tokens token;
        public IdentifierExpression Name { get; private set; }
        public IExpression Value { get; private set; }

        public string TokenLiteral()
        {
            throw new NotImplementedException();
        }

        public void StatementNode()
        {
            throw new NotImplementedException();
        }
    }
}
