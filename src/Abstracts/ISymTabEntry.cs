using System;
using System.Collections.Generic;

namespace OoxmlToHtml.Abstracts
{
    public interface ISymTabEntry
    {
        ISymTab SymTab { get; }
        string Name { get; }
        void AppendLineNumber(int lineNumber);
        IReadOnlyList<int> LineNumbers { get; }
        void Attribute(ISymTabKey key, object value);
        object GetAttribute(ISymTabKey key);
    }
}