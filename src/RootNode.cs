using OoxmlToHtml.Abstracts;

namespace OoxmlToHtml
{
    public class RootNode : IRootNode
    {
        public INode Root { get; private set; }
        public void SetRootNode(INode node)
        {
            Root = node;
        }
    }
}