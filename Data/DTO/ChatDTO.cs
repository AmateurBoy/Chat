using ChatMarchenkoIlya.Entitys;

namespace ChatMarchenkoIlya.Data.DTO
{
    public class ChatDTO
    {
        public int Id { get; set; }
        public string NameChat { get; set; }
       
        public List<string> NameUser { get; set; }
        public List<int> UserId { get; set; }
    }
}
