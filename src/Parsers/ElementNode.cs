﻿using System.Linq;
using OoxmlToHtml.Abstracts;
using OoxmlToHtml.Factories;

namespace OoxmlToHtml.Parsers
{
    public abstract class ElementNode : OoxmlNodeTd, IStatementParser
    {
        protected ElementNode(OoxmlNodeTd parent) : base(parent)
        {
        }
        protected abstract KeywordToken AttributeName { get; }
        protected virtual KeywordToken[] IgnoredTokens => null;
        public virtual INode Parse(Token token)
        {
            var m = NodeFactory.CreateNode(AttributeName);
            NextToken();
            while (CurrentToken.Keyword != KeywordToken.ENDING_ELEMENT
                   && CurrentToken.Keyword != KeywordToken.EOF
                   && CurrentToken.Keyword != KeywordToken.ShortClose)
            {
                IAttributeStatementParser attributeNode = null;
                switch (CurrentToken.Keyword)
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
                    m.SetAttribute(attributeNode.AttributeName,
                        attributeNode.Parse(CurrentToken).GetAttribute("value"));
                }
            }

            if (CurrentToken.Keyword == KeywordToken.ENDING_ELEMENT)
            {
                // we are in the body of the element
                NextToken();
                while (CurrentToken.Keyword != KeywordToken.EOF
                       && CurrentToken.Keyword != KeywordToken.Close)
                {
                    IStatementParser elementNode = null;
                    switch (CurrentToken.Keyword)
                    {
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
                       m.AddChild(elementNode.Parse(CurrentToken));
                    }
                    NextToken();
                }
            }
            return m;
        }
    }
}