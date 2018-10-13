using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        private List<int> _openTokens = new List<int> { -1 };
        public Token NextToken()
        {
            var token = scanner.NextToken();
            return token;
        }

        public Token PeekToken()
        {
            return scanner.PeekToken();
        }

        public Token CurrentToken => scanner.CurrentToken();
        public abstract void Parse(bool useDefaultAnalyzers = false);

        protected static ISymTab symTab = null;
        private ISymTabStack _symTabStack;
        public ISymTabStack SymTabStack => _symTabStack ?? (_symTabStack = SymTabFactory.CreateSymTabStack());
    }
}
