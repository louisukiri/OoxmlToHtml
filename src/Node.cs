using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OoxmlToHtml.Abstracts;

namespace OoxmlToHtml
{
    public class Node : Dictionary<string, string>, INode
    {
        private readonly HashSet<INode> _children = new HashSet<INode>();
        public INode Parent { get; private set; }
        public KeywordToken Type { get; }

        public IReadOnlyList<INode> Children => _children.ToList().AsReadOnly();   

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
            if (ContainsKey(name))
            {
                var keyIndex = 2;
                while (ContainsKey(name + "_" + keyIndex))
                {
                    keyIndex++;
                }

                name = name + "_" + keyIndex;
            }
            Add(name, value);
        }

        public string GetAttribute(string name) => this[name];
        public IReadOnlyDictionary<string, string> GetAllAttributes => new ReadOnlyDictionary<string, string>(this);

        public void SetParent(INode parent)
        {
            Parent = parent;
        }

        public void CopyChildren(INode source)
        {
            var childList = source.Children.ToList();
            foreach (var sourceChild in childList)
            {
                source.RemoveChild(sourceChild);
                AddChild(sourceChild);
            }
        }

        public void CopyAttributes(INode source)
        {
            foreach (var key in source.GetAllAttributes.Keys)
            {
                if (key.StartsWith("__"))
                {
                    continue;
                }
                SetAttribute(key, source.GetAttribute(key));
            }
        }

        public void RemoveChild(INode child)
        {
            _children.Remove(child);
        }

        public bool HasAttribute(string attributeName)
        {
            return this.ContainsKey(attributeName);
        }

        public void RemoveAttribute(string name)
        {
            Remove(name);
        }
    }
}