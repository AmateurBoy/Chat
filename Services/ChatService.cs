﻿using ChatMarchenkoIlya.Data;
using ChatMarchenkoIlya.Data.DTO;
using ChatMarchenkoIlya.Entitys;
using ChatMarchenkoIlya.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatMarchenkoIlya.Services
{
    public class ChatService
    {
        
        public string ConnectChat(int ChatID,string UserName)
        {
            using (ApplicationContext AC = new())
            {
                try
                {
                    User  u = AC.Users.FirstOrDefault(x => x.Name == UserName);
                    
                    
                        var Chat = AC.Chats.Include(x => x.Users).FirstOrDefault(x => x.Id == ChatID);  
                    
                        Chat.Users.Add(u);
                        
                    if (Chat.IsPrivate == false)
                    {
                        AC.Chats.Update(Chat);
                        AC.SaveChanges();
                        return $"Пользователь:->{u.Name} Подключен к чату -> {Chat.Name}";
                    }
                    else
                    {
                        Chat.IsPrivate = false;
                        AC.Chats.Update(Chat);
                        AC.SaveChanges();
                        return $"Чат больше не приватный.{u.Name} Подключен к чату -> {Chat.Name}";
                    }
                        
                }
                catch
                {
                    return "Подключение...Уже подключены";
                }
                
                
            }
        }
        public Chat AddChat(string NameChat,int userid)
        {
            using (ApplicationContext AC = new())
            {
                User user = AC.Users.FirstOrDefault(x => x.Id == userid);

                Chat findchat = new()
                {
                    Name = NameChat,
                    Users = new List<User>()
                    {
                        user
                    },                                 
                };
                AC.Chats.Add(findchat);            
                                       
                try
                {
                    AC.SaveChanges();
                    findchat = AC.Chats.FirstOrDefault(x => x.Name == findchat.Name);
                    return findchat;
                }
                catch
                {
                    Console.WriteLine("Сейв не удался");
                    return findchat;                    
                }               
            }
        }
        public Chat AddChat(string NameChat, int userid, bool IsPrivat)
        {
            using (ApplicationContext AC = new())
            {
                User user = AC.Users.FirstOrDefault(x => x.Id == userid);

                Chat findchat = new()
                {
                    Name = NameChat,
                    Users = new List<User>()
                    {
                        user
                    },
                    IsPrivate = IsPrivat,
                };
                AC.Chats.Add(findchat);

                try
                {
                    AC.SaveChanges();
                    findchat = AC.Chats.FirstOrDefault(x => x.Name == findchat.Name);
                    return findchat;
                }
                catch
                {
                    Console.WriteLine("Сейв не удался");
                    return findchat;
                }
            }
        }
        public string ExitChat(User user,int IdChat)
        {
            using (ApplicationContext AC = new())
            {
                var ChatUsers = AC.Chats.Include(x => x.Users).Where(x => x.Users.Count != 0).ToList();
                if(ChatUsers.Count>0)
                {
                    var Chat = ChatUsers.FirstOrDefault(x => x.Id == IdChat);
                    Chat.Users.Remove(Chat.Users.FirstOrDefault(x => x.Id == user.Id));
                    AC.Chats.Update(Chat);
                    AC.SaveChanges();
                    if (Chat != null)
                    {
                        return $"Вы вышли из чата {Chat.Name}";
                    }
                    else
                    {
                        return $"неизвестаня ошибка";
                    }
                }
                
                
                else
                {
                    return $"Выйти из чата не удалось.";
                }

            }
        }
        public Chat GetChat(int ChatID)
        {
            using (ApplicationContext AC = new())
            {
                return AC.Chats.Include(x=> x.Messages).Include(x => x.Users).FirstOrDefault(x => x.Id == ChatID);
            }
        }
        public List<ChatDTO> GetChatUser(User user)
        {
            using (ApplicationContext AC = new())
            {
                int count = 0;
                var ChatUsers = AC.Chats.Include(x => x.Users).ToList()
                    .Where(x => x.Users.OrderBy(x => x.Id == user.Id) != null);
                    

                var Test = AC.Chats.Include(x => x.Users).ToList()
                    .OrderBy(x=> x.Users.OrderBy(x=>x.Id==user.Id));


                var ChatDel = AC.Chats.Include(x => x.Users).Where(x => x.Users.Count == 0);
                if(ChatDel!=null)
                {
                    AC.Chats.RemoveRange(ChatDel);
                    AC.SaveChanges();
                }
                
                List <ChatDTO> chatDTOs = new List<ChatDTO>();
                foreach (var chat in ChatUsers.ToList())
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
                
                return chatDTOs;
            }            
        }
        public List<Message> GetChatMessages(Chat Chat)
        {
            
            using (ApplicationContext AC = new())
            {
                List<Message> messages = new List<Message>();
                int count = 0;
                var ChatMessage = AC.Chats.OrderBy(x => x.Id == Chat.Id)
                    .Include(x => x.Messages).ToList();
                
                foreach (var chat in ChatMessage)
                {
                    chat.Messages.OrderBy(x => x.dateTime);
                    messages = chat.Messages.ToList();
                }

                return messages;
            }
        }
    }
}
