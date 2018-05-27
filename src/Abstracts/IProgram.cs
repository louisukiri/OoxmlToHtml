using System.Collections.Generic;
using OoxmlToHtml.Abstracts.Ast;

namespace OoxmlToHtml.Abstracts
{
    public interface IProgram : INode
    {
        IReadOnlyList<IStatement> Statements { get; }
        void AddStatement(IStatement statement);
    }
}