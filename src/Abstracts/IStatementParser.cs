using System.Dynamic;

namespace OoxmlToHtml.Abstracts
{
    public interface IStatementParser
    {
        INode Parse(Token token);
    }
}