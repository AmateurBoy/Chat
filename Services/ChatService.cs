using ChatMarchenkoIlya.Data;
using ChatMarchenkoIlya.Data.DTO;
using ChatMarchenkoIlya.Entitys;
using ChatMarchenkoIlya.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatMarchenkoIlya.Services
{
    public class ChatService
    {
        public void AddChat(string NameChat,User usercreater)
        {
            using (ApplicationContext AC = new())
            {                
                
                Chat findchat = AC.Chats.FirstOrDefault(x => x.Id == 2);
                if(findchat==null)
                {
                    findchat.Users = new List<User>();
                    findchat.Users.Add(usercreater);
                    AC.Chats.Update(findchat);                        
                }
               
                try
                {
                    AC.SaveChanges();
                }
                catch
                {
                    Console.WriteLine("Сейв не удался");
                }
                                
            }
        }
        public List<ChatDTO> GetChatUser(User user)
        {
            using (ApplicationContext AC = new())
            {
                //var chatlist = user.Chats.Where(x => x.Id == user.Id);

                int count = 0;
                var ChatUsers = AC.Chats.Include(x => x.Users).ToList();
                List<ChatDTO> chatDTOs = new List<ChatDTO>();
                foreach (var chat in ChatUsers)
                {
                    if (chat.Users.Count != 0)
                    {
                        List<string> Name = new List<string>();
                        List<int> Id = new List<int>();
                        foreach (var item in chat.Users)
                        {
                            Name.Add(item.Name);
                            Id.Add(item.Id);
                        }
                        ChatDTO CDTO = new()
                        {
                            Id = chat.Id,
                            NameChat = chat.Name,
                            NameUser = Name,
                            UserId = Id
                        };
                        chatDTOs.Add(CDTO);
                    }
                }
                
                return chatDTOs;
            }            
        }
    }
}
