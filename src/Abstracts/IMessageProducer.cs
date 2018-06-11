namespace OoxmlToHtml.Abstracts
{
    public interface IMessageProducer
    {
        void AddMessageListener(IMessageListener listener);
        void RemoveMessageListener(IMessageListener listener);
        void SendMessage(IMessage message);
    }
}