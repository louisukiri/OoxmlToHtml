using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OoxmlToHtml.AST.abstracts;

namespace OoxmlToHtml
{
    public class Program : INode
    {
        private readonly List<IStatement> _backingList;
        public IReadOnlyList<IStatement> Statements => _backingList.AsReadOnly();

        public Program()
        {
            _backingList = new List<IStatement>();
        }
        public Program(List<IStatement> statements)
        {
            _backingList = statements;
        }
        public string TokenLiteral()
        {
            return Statements.Any() ? Statements[0].TokenLiteral() : "";
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void AddStatement(IStatement statement)
        {
            _backingList.Add(statement);
        }
    }
}
