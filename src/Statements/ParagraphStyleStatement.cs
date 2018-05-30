using OoxmlToHtml.Abstracts.Ast;

namespace OoxmlToHtml.Statements
{
    public class ParagraphStyleStatement : IStatement
    {
        public Token Token { get; private set; }
        private string _value = "";
        public ParagraphStyleStatement(Token token)
        {
            Token = token;
        }
        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void AddStatement(IStatement childStatement)
        {
            if (childStatement.Token.Type == Token.Value)
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