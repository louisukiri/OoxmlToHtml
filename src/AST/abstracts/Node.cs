using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace OoxmlToHtml.AST.abstracts
{
    public interface INode
    {
        string TokenLiteral();
        void Accept(IVisitor visitor);
    }
}
