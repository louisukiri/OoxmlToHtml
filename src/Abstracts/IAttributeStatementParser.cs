namespace OoxmlToHtml.Abstracts
{
    public interface IAttributeStatementParser : IStatementParser
    {
        string AttributeName { get; }
        KeywordToken Token { get; }
    }
}