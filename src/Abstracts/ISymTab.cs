using System.Collections;
using System.Collections.Generic;

namespace OoxmlToHtml.Abstracts
{
    public interface ISymTab
    {
        int NestingLevel { get; }
        ISymTabEntry Enter(string name);
        ISymTabEntry Lookup(string name);
        IList<ISymTabEntry> SortedEntries { get; }
    }
}