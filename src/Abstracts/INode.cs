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
        IReadOnlyDictionary<string, string> GetAllAttributes { get; }
        void SetParent(INode parent);
        void CopyChildren(INode source);
        void CopyAttributes(INode source);
        void RemoveChild(INode child);
        bool HasAttribute(string attributeName);
        void RemoveAttribute(string value);
    }
}