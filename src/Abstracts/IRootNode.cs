namespace OoxmlToHtml.Abstracts
{
    public interface IRootNode
    {
        void SetRootNode(INode node);
        INode Root { get; }
    }
}