using System;
using System.Collections;
using System.Collections.Generic;
using OoxmlToHtml.Abstracts;
using OoxmlToHtml.Abstracts.Ast;
using OoxmlToHtml.Statements;
using OoxmlToHtml.Visitors;

namespace OoxmlToHtml
{
    public abstract class Parser
    {
        private readonly Lexer _lexer;
        private Token _currentToken;
        private Token _peekToken;
        private IList<IAnalyzer> _analyzers = new List<IAnalyzer>();
        private readonly HtmlVisitor _visitor = new HtmlVisitor();
        protected static ISymTab symTab = null;
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
    }
}
