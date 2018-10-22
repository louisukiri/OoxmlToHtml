using System.Collections.Generic;

namespace OoxmlToHtml.Abstracts
{
    public interface INode
    {
        INode AddChild(INode child);
        INode Parent { get; }
        INode Previous { get; }
        INode Next { get; }
        INode Child { get; }
        KeywordToken Type { get; }
        IReadOnlyList<INode> Children { get; }
        bool SetAttribute(string name, string value, AttributeMergeStrategy strategy = AttributeMergeStrategy.Rename);
        bool CanSetAttribute(string name, string value,
            AttributeMergeStrategy strategy = AttributeMergeStrategy.Rename);
        string GetAttribute(string name);
        IReadOnlyDictionary<string, string> GetAllAttributes { get; }
        void SetParent(INode parent);
        void SetPrev(INode prevNode);
        void SetNext(INode nextNode);
        void CopyChildren(INode source);
        void CopyAttributes(INode source);
        void RemoveChild(INode child);
        bool HasAttribute(string attributeName);
        void RemoveAttribute(string value);
    }
}