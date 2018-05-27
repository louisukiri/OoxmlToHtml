namespace OoxmlToHtml.Abstracts.Ast
{
    public interface INode
    {
        string TokenLiteral();
        void Accept(IVisitor visitor);
    }
}
