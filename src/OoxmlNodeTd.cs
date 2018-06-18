using System.Collections.Generic;
using System.Net.Http.Headers;
using OoxmlToHtml.Abstracts;
using OoxmlToHtml.Analyzers;
using OoxmlToHtml.Factories;
using OoxmlToHtml.Parsers;
using OoxmlToHtml.Tokens;

namespace OoxmlToHtml
{
    public class OoxmlNodeTd : Parser
    {
        public IRootNode Root { get; private set; }
        private Analyzer _analyzers = null;
        public OoxmlNodeTd(Scanner scanner) : base(scanner)
        {
        }

        public OoxmlNodeTd(OoxmlNodeTd parent) : base(parent.scanner)
        {

        }

        public override void Parse(bool useDefaultAnalyzers = false)
        {
            NextToken();
            Root = NodeFactory.CreateRootNode();

            while (CurrentToken.Keyword != KeywordToken.EOF)
            {
                // only container elements can be processed here
                switch (CurrentToken.Keyword)
                {
                    case KeywordToken.Paragraph:
                        var a = new ParagraphStatementParser(this);
                        Root.SetRootNode(a.Parse(CurrentToken));
                        break;
                    case KeywordToken.Body:
                        var b = new GenericElementNode(this, KeywordToken.Body);
                        Root.SetRootNode(b.Parse(CurrentToken));
                        break;
                }

                NextToken();
            }

            if (useDefaultAnalyzers)
            {
                if (_analyzers == null)
                {
                    _analyzers = new ElementToAttributeAnalyzer();
                    _analyzers.Use(new AttributeCopierAnalyzer());
                }
                else
                {
                    var elementAnalyzer = new ElementToAttributeAnalyzer();
                    elementAnalyzer.Use(new AttributeCopierAnalyzer())
                                   .Use(_analyzers);
                    _analyzers = elementAnalyzer;
                }
            }
            if (_analyzers == null) return;
            Root.SetRootNode(_analyzers.Analyze(Root.Root));
        }

        public void Use(Analyzer analyzer)
        {
            if (_analyzers == null)
            {
                _analyzers = analyzer;
                return;
            }

            _analyzers.Use(analyzer);
        }
    }
}