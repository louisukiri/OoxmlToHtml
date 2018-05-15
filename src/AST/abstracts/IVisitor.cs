using System;
using System.Collections.Generic;
using System.Text;
using OoxmlToHtml.Statements;

namespace OoxmlToHtml.AST.abstracts
{
    public interface IVisitor
    {
        void Visit(ColorStatement statement);
        void Visit(AttributeStatement statement);
        void Visit(ParagraphPropertyStatement statement);
        void Visit(RunStatement statement);
        void Visit(StringStatement statement);
        void Visit(IdentifierExpression expression);
        void Visit(Program program);
        void Visit(ParagraphStatement statement);
        void Visit(SizeStatement statement);
    }
}
