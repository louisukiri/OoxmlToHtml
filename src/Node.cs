using System.Collections.Generic;
using OoxmlToHtml.Abstracts;

namespace OoxmlToHtml
{
    public class Node : Dictionary<string, string>, INode
    {
        private readonly List<INode> _children = new List<INode>();
        public INode Parent { get; private set; }
        public KeywordToken Type { get; }

        public IReadOnlyList<INode> Children => _children.AsReadOnly();   

        public Node(KeywordToken tokenType)
        {
            Type = tokenType;
            Parent = null;
        }
        public INode AddChild(INode child)
        {
            _children.Add(child);
            
            return child;
        }

        public void SetAttribute(string name, string value)
        {
            Add(name, value);
        }

        public string GetAttribute(string name) => this[name];

        public void SetParent(INode parent)
        {
            Parent = parent;
        }
    }
}