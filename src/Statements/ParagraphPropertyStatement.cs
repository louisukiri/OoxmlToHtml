﻿using System;
using System.Collections.Generic;
using System.Text;
using OoxmlToHtml.AST.abstracts;

namespace OoxmlToHtml.Statements
{
    public class ParagraphPropertyStatement : IStatement
    {
        public string TokenLiteral()
        {
            throw new NotImplementedException();
        }

        public Tokens Token { get; }

        public ParagraphPropertyStatement(Tokens token)
        {
            Token = token;
        }
        public void StatementNode()
        {
            throw new NotImplementedException();
        }

        public void AddStatement(IStatement childStatement)
        {
            throw new NotImplementedException();
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}