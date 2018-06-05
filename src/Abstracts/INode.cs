using System.Collections.Generic;

namespace OoxmlToHtml.Abstracts
{
    public interface INode
    {
        INode AddChild(INode child);
        INode Parent { get; }
        KeywordToken Type { get; }
        IReadOnlyList<INode> Children { get; }
        void SetAttribute(string name, string value);
        string GetAttribute(string name);
        void SetParent(INode parent);
    }
}