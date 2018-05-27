namespace OoxmlToHtml.Abstracts
{
    public interface IParser
    {
        IProgram ParseProgram();
        IParser Use(IAnalyzer analyzer);
        void Analyze();
    }
}