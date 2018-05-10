using System;
using System.Collections.Generic;
using System.Text;
using OoxmlToHtml.AST.abstracts;

namespace OoxmlToHtml.Statements
{
    public class ParagraphStatement : IStatement
    {
        private List<IStatement> _properties;
        private List<IStatement> _annotations;
        private List<IStatement> _run;
        private List<IStatement> _custom;
        private readonly List<IStatement> _statements = new List<IStatement>();

        public IReadOnlyCollection<IStatement> Statements => _statements.AsReadOnly();
        public IReadOnlyCollection<IStatement> Properties => _properties.AsReadOnly();
        public IReadOnlyCollection<IStatement> Annotations => _annotations.AsReadOnly();
        public IReadOnlyCollection<IStatement> Run => _run.AsReadOnly();
        public IReadOnlyCollection<IStatement> Custom => _custom.AsReadOnly();

        public Tokens Token { get; private set; }

        public ParagraphStatement(Tokens token)
        {
            Token = token;
        }
        public void AddStatement(IStatement statement)
        {
            if (statement == null) return;
            if (statement.Token.Type == Tokens.PreviousParagraph || statement.Token.Type == Tokens.Run)
                _statements.Add(statement);
        }

        public void StatementNode()
        {
            throw new NotImplementedException();
        }

        public string TokenLiteral()
        {
            throw new NotImplementedException();
        }
    }
}
