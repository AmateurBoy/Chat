namespace ChatMarchenkoIlya.Entitys
{
    public enum TypeChat { Private, Public };
    public class Chat
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsPrivate { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}
