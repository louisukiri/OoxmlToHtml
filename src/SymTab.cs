using System.Collections.Generic;
using System.Linq;
using OoxmlToHtml.Abstracts;
using OoxmlToHtml.Factories;

namespace OoxmlToHtml
{
    public class SymTab : SortedDictionary<string, ISymTabEntry>, ISymTab
    {
        public int NestingLevel { get; }
        public ISymTabEntry Enter(string name)
        {
            var entry = SymTabFactory.CreateSymTabEntry(name, this);
            Add(name, entry);

            return entry;
        }

        public ISymTabEntry Lookup(string name)
        {
            return this[name];
        }

        public IList<ISymTabEntry> SortedEntries => Values.ToList();

        public SymTab(int nestingLevel)
        {
            NestingLevel = nestingLevel;
        }
    }
}