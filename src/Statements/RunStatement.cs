using System;
using System.Collections.Generic;
using System.Text;
using OoxmlToHtml.AST.abstracts;
using OoxmlToHtml.Flags;

namespace OoxmlToHtml.Statements
{
    public class RunStatement : IStatement
    {
        public Tokens Token { get; }
        public string Text { get; private set; }
        public Styles styles { get; private set; }

        public RunStatement(Tokens token)
        {
            Token = token;
            Text = string.Empty;
        }

        public string TokenLiteral()
        {
            return Text;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void StatementNode()
        {
            throw new NotImplementedException();
        }

        public void AddStatement(IStatement childStatement)
        {
            if (childStatement.Token.Type == Tokens.StringLiteral)
            {
                AddSpace();
                Text += childStatement.TokenLiteral();
            }

            if (childStatement.Token.Type == Tokens.Bold)
            {
                styles |= Styles.Bold;
            }

            if (childStatement.Token.Type == Tokens.Italic)
            {
                styles |= Styles.Italic;
            }
        }

        private void AddSpace()
        {
            if (string.IsNullOrEmpty(Text))
                return;
            Text += " ";
        }
    }
}
