namespace OoxmlToHtml.Abstracts.Ast
{
    public interface IStatement : INode
    {
        Tokens Token { get; }
        void StatementNode();
        void AddStatement(IStatement childStatement);
    }
}
