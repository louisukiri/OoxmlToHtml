using OoxmlToHtml.Abstracts;

namespace OoxmlToHtml.Factories
{
    public class NodeFactory
    {
        public static IRootNode CreateRootNode() => new RootNode();
        public static INode CreateNode(KeywordToken type) => new Node(type);
    }
}