﻿using System.Collections.Generic;
using System.Linq;
using OoxmlToHtml.Abstracts;
using OoxmlToHtml.Abstracts.Ast;

namespace OoxmlToHtml
{
    public class Program : IProgram
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
