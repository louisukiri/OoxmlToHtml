using System;
using System.Linq;
using OoxmlToHtml.Abstracts;
using OoxmlToHtml.Factories;

namespace OoxmlToHtml.Parsers
{
    public abstract class ElementNode : OoxmlNodeTd, IStatementParser
    {
        protected ElementNode(OoxmlNodeTd parser) : base(parser)
        {
        }
        protected abstract KeywordToken AttributeName { get; }
        protected virtual KeywordToken[] IgnoredTokens => null;
        public virtual INode Parse(Token token)
        {
            var currentNode = NodeFactory.CreateNode(AttributeName);
            NextToken();
            while (parser.CurrentToken.Keyword != KeywordToken.ENDING_ELEMENT
                   && parser.CurrentToken.Keyword != KeywordToken.EOF
                   && parser.CurrentToken.Keyword != KeywordToken.ShortClose)
            {
                IAttributeStatementParser attributeNode = null;
                switch (parser.CurrentToken.Keyword)
                {
                    // parse attributes
                    case KeywordToken.StringLiteral:
                        attributeNode = new UnknownStringLiteralAttribute(this);
                        break;
                    case KeywordToken.Value:
                        attributeNode = new ValueAttribute(this);
                        break;
                    default:
                        NextToken();
                        break;
                }

                if (attributeNode != null)
                {
                    currentNode.SetAttribute(attributeNode.AttributeName,
                        attributeNode.Parse(parser.CurrentToken).GetAttribute("value"));
                }
            }
            // if it's an EOF it's unexpected err out
            if (parser.CurrentToken.Keyword == KeywordToken.EOF) throw new Exception("Invalid end of file");

            if (parser.CurrentToken.Keyword != KeywordToken.ENDING_ELEMENT)
            {
                parser.NextToken();
                return currentNode;
            }
            // we are in the body of the element
            parser.NextToken();
            // if we are not at the end of the file or at the end tag of the of the current tag. go into the while
            // if we are at the end of the current tag, stop collecting children for the current tag
            while (parser.CurrentToken.Keyword != KeywordToken.EOF
                   && (!(parser.CurrentToken.Keyword == KeywordToken.Close
                         && parser.CurrentToken.Text == token.Text))
            )
            {
                IStatementParser elementNode = null;
                switch (parser.CurrentToken.Keyword)
                {
                    case KeywordToken.Code:
                        elementNode = new CodeParser(this);
                        break;
                    case KeywordToken.StringLiteral:
                        elementNode = new StringLiteralStatementParser(this);
                        break;
                    case KeywordToken.Paragraph:
                        elementNode = new ParagraphStatementParser(this);
                        break;
                    case KeywordToken.PreviousParagraph:
                        elementNode = new PreviousParagraphStatementParser(this);
                        break;
                    case KeywordToken.Color:
                        elementNode = new ColorStatementNode(this);
                        break;
                    case KeywordToken.Text:
                        elementNode = new TextStatementParser(this);
                        break;
                    case KeywordToken.Italic:
                        elementNode = new ItalicStatementParser(this);
                        break;
                    case KeywordToken.ParagraphStyle:
                        elementNode = new ParagraphStyleStatementParser(this);
                        break;
                    case KeywordToken.Unknown:
                        elementNode = new UnknownElementParser(this);
                        break;
                    case KeywordToken.Bold:
                        elementNode = new GenericElementNode(this, KeywordToken.Bold);
                        break;
                    case KeywordToken.Run:
                        elementNode = new GenericElementNode(this, KeywordToken.Run);
                        break;
                    case KeywordToken.Size:
                        elementNode = new GenericElementNode(this, KeywordToken.Size);
                        break;
                }

                if (elementNode != null)
                {
                    currentNode.AddChild(elementNode.Parse(CurrentToken));
                }
                else parser.NextToken();
            }
            parser.NextToken();
            return currentNode;
        }
    }
}