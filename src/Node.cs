using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Moq;
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
        public INode Parent { get; private set; }
        public INode Previous { get; private set; }
        public INode Next { get; private set; }
        public KeywordToken Type { get; }

        public IReadOnlyList<INode> Children {
            get
            {
                var ch = Child;
                List<INode> nodes = new List<INode>();
                if (ch == null)
                {
                    return nodes.AsReadOnly();
                }
                nodes.Add(ch);
                while (ch.Next != null)
                {
                    nodes.Add(ch.Next);
                    ch = ch.Next;
                }
                return nodes.AsReadOnly();
            }
        }

        public Node(KeywordToken tokenType)
        {
            Type = tokenType;
            Parent = null;
        }
        public INode AddChild(INode child)
        {
            if (child == null)
            {
                return null;
            }
            if (this.Child == child)
            {
                return child;
            }

            var currentChild = this.Child;
            if (currentChild == null)
            {
                this.Child = child;
            }
            else
            {
                while (currentChild.Next != null)
                {
                    currentChild = currentChild.Next;
                }
                currentChild.SetNext(child);
            }
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
            var alreadyContainsKey = ContainsKey(name);
            var valueIsNotIdentical = GetAttributeOrEmpty(name) != value;
            var doesNotHaveStyle = !ContainsKey("style");
            if (name == "Text") mergeStrategy = AttributeMergeStrategy.Append;
            var canSetAttribute = !(alreadyContainsKey 
                                    && mergeStrategy == AttributeMergeStrategy.Merge
                                    && valueIsNotIdentical
                                    && doesNotHaveStyle);
            return canSetAttribute;
        }

        public string GetAttribute(string name) => this[name];
        public string GetAttributeOrEmpty(string name) => ContainsKey(name) ? this[name] : string.Empty;
        public IReadOnlyDictionary<string, string> GetAllAttributes => new ReadOnlyDictionary<string, string>(this);

        public INode Child { get; set; }

        public void SetParent(INode parent)
        {
            Parent = parent;
        }

        public void SetPrev(INode prevNode)
        {
            Previous = prevNode;
            if (Previous == null)
            {
                return;
            }
            if (prevNode.Next == this) return;
            prevNode.SetNext(this);
        }

        public void SetNext(INode nextNode)
        {
            Next = nextNode;
            if (Next == null) return;
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
            if (child == this.Child)
            {
                this.Child = child.Next;
                return;
            }
            var childToRemove = Children.FirstOrDefault(z => child == z);
            childToRemove?.Previous.SetNext(childToRemove.Next);
        }

        public bool HasAttribute(string attributeName)
        {
            return this.ContainsKey(attributeName);
        }
        
        public void RemoveAttribute(string name)
        {
            Remove(name);
        }

        public override bool Equals(object obj)
        {
            var rhs = obj as Node;
            var lhs = this;
            if (rhs == null) return false;
            if (lhs.Type != rhs.Type) return false;
            if (lhs.Count != rhs.Count) return false;
            //var lhsAttribs = lhs.GetAllAttributes;
            //var rhsAttribs = rhs.GetAllAttributes;

            foreach (string key in lhs.Keys)
            {
                if (!rhs.ContainsKey(key)) return false;
                if (rhs[key] != lhs[key]) return false;
            }

            return true;
        }

        public override int GetHashCode() => base.GetHashCode();

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"<{this.Type.ToString().ToLower()}");
            foreach (var attribute in GetAllAttributes.Keys)
            {
                builder.Append($" {attribute.ToLower()}='{this.GetAttribute(attribute)}'");
            }
            if (Child != null)
            {
                builder.Append($">");
                builder.Append(Child);
                builder.Append($"</{Type.ToString().ToLower()}>");
            }
            else builder.Append($" />");
            if (Next != null)
                builder.Append(Next);
            return builder.ToString();
        }
    }
}