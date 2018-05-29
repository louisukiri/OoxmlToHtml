using OoxmlToHtml.Abstracts.Ast;

namespace OoxmlToHtml.Statements
{
    public class ParagraphStyleStatement : IStatement
    {
        public Tokens Token { get; private set; }
        private string _value = "";
        public ParagraphStyleStatement(Tokens tokens)
        {
            Token = tokens;
        }
        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void AddStatement(IStatement childStatement)
        {
            if (childStatement.Token.Type == Tokens.Value)
            {
                _value = childStatement.TokenLiteral();
            }
        }

        public void StatementNode()
        {
        }

        public string TokenLiteral()
        {
            return _value;
        }
    }
}