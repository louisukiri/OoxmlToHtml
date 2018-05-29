using OoxmlToHtml.Abstracts.Ast;

namespace OoxmlToHtml.Statements
{
    public class HeaderStatement : IStatement
    {
        public Tokens Token { get; private set; }
        private string _value = string.Empty;
        private string _name = string.Empty;
        public HeaderStatement(Tokens tokens)
        {
            Token = tokens;
        }
        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void AddStatement(IStatement childStatement)
        {
            if (childStatement.Token.Type != Tokens.StringLiteral) return;
            _value = childStatement.TokenLiteral();
        }

        public void StatementNode()
        {
            throw new System.NotImplementedException();
        }

        public string TokenLiteral()
        {
            return _value;
        }
    }
}
