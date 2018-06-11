using System.Collections;
using System.Collections.Generic;
using OoxmlToHtml.Abstracts;
using OoxmlToHtml.Factories;

namespace OoxmlToHtml
{
    public class SymTabStack : List<ISymTab>, ISymTabStack
    {
        private IList<ISymTab> _underLyingList = null;

        public int CurrentNestingLevel { get; private set; }

        public ISymTab LocalSymTab => this[CurrentNestingLevel];

        public ISymTabEntry EnterLocal(string name)
        {
            return LocalSymTab.Enter(name);
        }

        public ISymTabEntry Lookup(string name)
        {
            return LookupLocal(name);
        }

        public ISymTabEntry LookupLocal(string name)
        {
            return LocalSymTab.Lookup(name);
        }
        
        public SymTabStack()
        {
            _underLyingList = new List<ISymTab>();
            CurrentNestingLevel = 0;
            Add(SymTabFactory.CreateSymTab(CurrentNestingLevel));
        }
    }
}