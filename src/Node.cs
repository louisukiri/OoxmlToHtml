using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using OoxmlToHtml.Abstracts;

namespace OoxmlToHtml
{
    public enum AttributeMergeStrategy
    {
        Merge,
        Overwrite,
        Append,
        Rename,
        Skip
    }
    public class Node : Dictionary<string, string>, INode
    {
        private readonly HashSet<INode> _children = new HashSet<INode>();
        public INode Parent { get; private set; }
        public INode Previous { get; private set; }
        public INode Next { get; private set; }
        public KeywordToken Type { get; }

        public IReadOnlyList<INode> Children => _children.ToList().AsReadOnly();   

        public Node(KeywordToken tokenType)
        {
            Type = tokenType;
            Parent = null;
        }
        public INode AddChild(INode child)
        {
            var previousChild = _children.LastOrDefault();
            var childAttr = child.GetAllAttributes;
            if (child.Type == KeywordToken.Run
                && previousChild != null 
                && previousChild.Type == child.Type
                && childAttr.Keys.All(attribute =>
                    previousChild.CanSetAttribute(attribute, childAttr[attribute], AttributeMergeStrategy.Merge)))
            {
                foreach (var childAttrKey in childAttr.Keys)
                {
                    previousChild.SetAttribute(childAttrKey, childAttr[childAttrKey], AttributeMergeStrategy.Merge);
                }
                previousChild.CopyChildren(child);
                return previousChild;
            }
            _children.Add(child);
            child.SetParent(this);
            return child;
        }

        public bool SetAttribute(string name, string value, AttributeMergeStrategy mergeStrategy=AttributeMergeStrategy.Rename)
        {
            if (ContainsKey(name))
            {
                if (name == "Text") mergeStrategy = AttributeMergeStrategy.Append;
                if (name == "style" && value == this[name])
                {
                    mergeStrategy = AttributeMergeStrategy.Skip;
                }
                switch (mergeStrategy)
                {
                    case AttributeMergeStrategy.Merge:
                        if (value != GetAttribute(name)) return false;
                        return true;
                    case AttributeMergeStrategy.Overwrite:
                        RemoveAttribute(name);
                        break;
                    case AttributeMergeStrategy.Append:
                        value = GetAttribute(name) + value;
                        RemoveAttribute(name);
                    break;
                    case AttributeMergeStrategy.Skip:
                        return true;
                    default:
                        var keyIndex = 2;
                        while (ContainsKey(name + "_" + keyIndex))
                        {
                            keyIndex++;
                        }
                        name = name + "_" + keyIndex;
                   break;
                }
            }
            Add(name, value);
            return true;
        }

        public bool CanSetAttribute(string name, string value, AttributeMergeStrategy mergeStrategy = AttributeMergeStrategy.Rename)
        {
            return !(ContainsKey(name) && mergeStrategy == AttributeMergeStrategy.Merge
                                     && value != GetAttribute(name));
        }

        public string GetAttribute(string name) => this[name];
        public IReadOnlyDictionary<string, string> GetAllAttributes => new ReadOnlyDictionary<string, string>(this);

        public void SetParent(INode parent)
        {
            Parent = parent;
        }

        public void SetPrev(INode prevNode)
        {
            Previous = prevNode;
            if (prevNode.Next == this) return;
            prevNode.SetNext(this);
        }

        public void SetNext(INode nextNode)
        {
            Next = nextNode;
            if (nextNode.Next == this) return;
            nextNode.SetPrev(this);
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

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"<{this.Type.ToString()} ");
            foreach (var attribute in GetAllAttributes.Keys)
            {
                builder.Append($" {attribute}='{this.GetAttribute(attribute)} '");
            }
            builder.Append($">");
            foreach (var child in _children)
            {
                builder.Append(child.ToString());
            }
            builder.Append($"</{this.Type.ToString()}>");
            return builder.ToString();
        }
    }
}