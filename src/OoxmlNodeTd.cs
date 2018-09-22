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
        protected OoxmlNodeTd parser;
        public OoxmlNodeTd(Scanner scanner) : base(scanner)
        {
            parser = this;
        }

        public OoxmlNodeTd(OoxmlNodeTd parser) : base(parser.scanner)
        {
            this.parser = parser;
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
                        var a = new ParagraphStatementParser(parser);
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
                    _analyzers = new ChildrenAnalyzer();
                    _analyzers.Use(new ElementToAttributeAnalyzer())
                              .Use(new AttributeCopierAnalyzer())
                              .Use(new SiblingsAnalyzer())
                        ;
                }
                else
                {
                    var elementAnalyzer = new ChildrenAnalyzer();
                    elementAnalyzer.Use(new ElementToAttributeAnalyzer())
                                   .Use(new AttributeCopierAnalyzer())
                                   .Use(new SiblingsAnalyzer())
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