namespace OoxmlToHtml.Abstracts
{
    public interface IParser
    {
        IProgram ParseProgram();
        IParser Use(IAnalyzer analyzer);
        IParseResult Parse();
        IProgram Analyze(IProgram program);
    }
}