namespace ChatMarchenkoIlya.Interfaces
{
    public interface IMessage<T>
    {
        public void Sending(T From,T In,string Text);
        public void Editor(int IdMessage, string NewText);
        public void DelMessage(int IdMessage,bool IsPrivate);
        public void Reply(int IdMessage,T From,T In,string Text,bool IsPrivate);
    }
}
