﻿using System;
using System.Collections.Generic;
using System.Text;
using OoxmlToHtml.AST.abstracts;

namespace OoxmlToHtml.Statements
{
    public class ColorStatement : IStatement
    {
        public Tokens Token { get; private set; }
        public string Value { get; private set; }
        
        public ColorStatement(Tokens token, string value)
        {
            Token = token;
            Value = value;
        }

        public string TokenLiteral()
        {
            return Value;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void StatementNode()
        {
        }

        public void AddStatement(IStatement childStatement)
        {
            throw new NotImplementedException();
        }
    }
}
