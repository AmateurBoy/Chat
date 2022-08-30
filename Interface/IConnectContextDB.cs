using ChatMarchenkoIlya.Data;

namespace ChatMarchenkoIlya.Interface
{
    public interface IConnectContextDB
    {
        public ContextSingle Connect()
        {
            ApplicationContext AC = new();
            return ContextSingle.getInstance(AC);
        }
    }
}
