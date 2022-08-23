namespace ChatMarchenkoIlya.Entitys
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime dateTime { get; set; }
        public ICollection<User> User { get; set; }
        public ICollection<Chat> Chat { get; set; }

    }
}
