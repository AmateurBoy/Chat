using ChatMarchenkoIlya.Data;
using ChatMarchenkoIlya.Data.DTO;
using ChatMarchenkoIlya.Entitys;

using Microsoft.EntityFrameworkCore;

namespace ChatMarchenkoIlya.Services
{
    
    public class ChatService
    {
        
       
        public async Task<string> ConnectChat(int ChatID,string UserName)
        {
            using var AC = new ApplicationContext();

            try
                {
                    User u = await AC.Users.FirstOrDefaultAsync(x => x.Name == UserName);


                    var Chat = await AC.Chats.Include(x => x.Users).FirstOrDefaultAsync(x => x.Id == ChatID);

                    Chat.Users.Add(u);

                    if (Chat.IsPrivate == false)
                    {
                        AC.Chats.Update(Chat);
                        await AC.SaveChangesAsync();
                        return $"Пользователь:->{u.Name} Подключен к чату -> {Chat.Name}";
                    }
                    else
                    {
                        Chat.IsPrivate = false;
                        AC.Chats.Update(Chat);
                        await AC.SaveChangesAsync();
                        return $"Чат больше не приватный.{u.Name} Подключен к чату -> {Chat.Name}";
                    }

                }
                catch
                {
                    return "Подключение...Уже подключены";
                }
            
                       
        }
        public async Task<Chat> AddChat(string NameChat,int userid)
        {
            using var AC = new ApplicationContext();

            User user = await AC.Users.FirstOrDefaultAsync(x => x.Id == userid);

                Chat findchat = new()
                {
                    Name = NameChat,
                    Users = new List<User>()
                {
                    user
                },
                };
                await AC.Chats.AddAsync(findchat);

                try
                {
                    await AC.SaveChangesAsync();
                    findchat = await AC.Chats.FirstOrDefaultAsync(x => x.Name == findchat.Name);
                    return findchat;
                }
                catch
                {
                    Console.WriteLine("Сейв не удался");
                    return findchat;
                }
            
        }
        public async Task<Chat> AddChat(string NameChat, int userid, bool IsPrivat)
        {
            using var AC = new ApplicationContext();

            User user = await AC.Users.FirstOrDefaultAsync(x => x.Id == userid);

                Chat findchat = new()
                {
                    Name = NameChat,
                    Users = new List<User>()
                {
                    user

                },
                    IsPrivate = IsPrivat,
                };
                Chat chat = await AC.Chats.FirstOrDefaultAsync(x => x.Name == NameChat);
                if (chat == null)
                {
                    AC.Chats.Add(findchat);
                    try
                    {
                        await AC.SaveChangesAsync();
                        findchat = await AC.Chats.FirstOrDefaultAsync(x => x.Name == findchat.Name);
                        return findchat;
                    }
                    catch
                    {
                        Console.WriteLine("Сейв не удался");
                        return findchat;
                    }
                }
                else
                {
                    return await GetChat(chat.Id);
                }
            
        }
        public async Task<Chat> AddChat(string NameChat, int userid, User user2, bool IsPrivat)
        {
            using var AC = new ApplicationContext();

            User user = await AC.Users.FirstOrDefaultAsync(x => x.Id == userid);

            Chat findchat = new()
            {
                Name = NameChat,
                Users = new List<User>()
                {
                    user,
                    user2
                },
                    
                    Messages = new List<Message>(),
                    

                    IsPrivate = IsPrivat,
                };
            findchat.Messages = user2.Messages;
                Chat chat = await AC.Chats.FirstOrDefaultAsync(x => x.Name == NameChat);
                if (chat == null)
                {
                    AC.Chats.Update(findchat); 
                    try
                    {
                        await AC.SaveChangesAsync();
                        findchat = await AC.Chats.Include(x=>x.Messages).FirstOrDefaultAsync(x => x.Name == findchat.Name);
                        return findchat;
                    }
                    catch
                    {
                        Console.WriteLine("Сейв не удался");
                        return findchat;
                    }
                }
                else
                {
                    return await GetChat(chat.Id);
                }
            
            
        }
        public async Task<Chat> AddMesgChat(Message msg,Chat c)
        {
            using var AC = new ApplicationContext();
            c.Messages = new List<Message>();
            c.Messages.Add(msg);
            AC.Update(c);
            await AC.SaveChangesAsync();
            return c;

        }
        public string ExitChat(User user,int IdChat)
        {
            using var AC = new ApplicationContext();

            var ChatUsers = AC.Chats.Include(x => x.Users).Where(x => x.Users.Count != 0).ToList();
                if (ChatUsers.Count > 0)
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
        public async Task<Chat> GetChat(int ChatID)
        {
            using var AC = new ApplicationContext();

            return await AC.Chats
                .Include(x => x.Messages)
                .Include(x => x.Users)
                .FirstOrDefaultAsync(x => x.Id == ChatID);
            
        }
        
    }
}
