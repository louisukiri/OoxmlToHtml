using System.Collections.Generic;
using OoxmlToHtml.Abstracts;

namespace OoxmlToHtml
{
    public class SymTabEntry : Dictionary<ISymTabKey, object>, ISymTabEntry
    {
        public ISymTab SymTab { get; }
        public string Name { get; }
        private List<int> lineNumbers { get; }

        public IReadOnlyList<int> LineNumbers => lineNumbers.AsReadOnly();

        public void AppendLineNumber(int lineNumber)
        {
            lineNumbers.Add(lineNumber);
        }

        public void Attribute(ISymTabKey key, object value)
        {
            Add(key, value);
        }

        public object GetAttribute(ISymTabKey key)
        {
            return this[key];
        }

        public SymTabEntry(string name, ISymTab symTab)
        {
            Name = name;
            SymTab = symTab;
            lineNumbers = new List<int>();
        }
    }
}