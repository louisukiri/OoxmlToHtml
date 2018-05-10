using System;
using System.Collections.Generic;
using System.Text;

namespace OoxmlToHtml.AST.abstracts
{
    public interface IStatement : INode
    {
        Tokens Token { get; }
        void StatementNode();
        void AddStatement(IStatement childStatement);
    }
}
