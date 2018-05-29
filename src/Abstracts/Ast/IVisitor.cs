using OoxmlToHtml.Statements;

namespace OoxmlToHtml.Abstracts.Ast
{
    public interface IVisitor
    {
        void Visit(ColorStatement statement);
        void Visit(AttributeStatement statement);
        void Visit(ParagraphPropertyStatement statement);
        void Visit(RunStatement statement);
        void Visit(StringStatement statement);
        void Visit(IdentifierExpression expression);
        void Visit(ParagraphStyleStatement paragraphStyleStatement);
        void Visit(IProgram program);
        void Visit(ParagraphStatement statement);
        void Visit(SizeStatement statement);
        void Visit(HeaderStatement statement);
    }
}
