namespace OoxmlToHtml.Abstracts.Ast
{
    public interface IStatement : INode
    {
        Token Token { get; }
        void StatementNode();
        void AddStatement(IStatement childStatement);
    }
}
