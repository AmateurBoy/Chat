namespace ChatMarchenkoIlya.Entitys
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Chat> Chats {get;set;}
    }
}
