using System;
using System.Collections;
using System.Collections.Generic;
using OoxmlToHtml.Abstracts;
using OoxmlToHtml.Factories;

namespace OoxmlToHtml
{
    public abstract class Parser
    {
        private IList<IAnalyzer> _analyzers = new List<IAnalyzer>();
        protected Scanner scanner;
        protected ICode iCode;

        protected Parser(Scanner scanner)
        {
            this.scanner = scanner;
        }

        public Token NextToken()
        {
            return scanner.NextToken();
        }
        public Token CurrentToken => scanner.CurrentToken();
        public abstract void Parse();

        protected static ISymTab symTab = null;
        private ISymTabStack _symTabStack;
        public ISymTabStack SymTabStack => _symTabStack ?? (_symTabStack = SymTabFactory.CreateSymTabStack());
    }
}
