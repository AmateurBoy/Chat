using ChatMarchenkoIlya.Data;
using ChatMarchenkoIlya.Entitys;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ChatMarchenkoIlya.Services
{
    public class MessageService
    {
       
        public async Task<Message> GetMessage(int mesgId)
        {
            using var AC = new ApplicationContext();
            return await AC.Messages.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == mesgId);
            
        }
        public async Task<Chat> CreateMessage(string Text, int UserId,int ChatId)
        {
            using var AC = new ApplicationContext();

            Chat chat = await AC.Chats.Include(x => x.Messages).Include(x => x.Users).FirstOrDefaultAsync(x => x.Id == ChatId);

                Message Mesg = new()
                {
                    Text = Text,
                    dateTime = DateTime.Now,
                    User = await AC.Users.Include(x => x.Chats).FirstOrDefaultAsync(x => x.Id == UserId),
                    Chat = new List<Chat>()
                {
                    chat
                }
                };

                await AC.Messages.AddAsync(Mesg);
                await AC.SaveChangesAsync();
                return chat;
            
            
        }
        public async Task<Chat> CreateMessageReply(string Text, int UserId, int ChatId)
        {
            using var AC = new ApplicationContext();

            Chat chat = await AC.Chats.Include(x => x.Messages).Include(x => x.Users).FirstOrDefaultAsync(x => x.Id == ChatId);

                Message Mesg = new()
                {
                    Text = Text,
                    Reply = true,
                    dateTime = DateTime.Now,
                    User = await AC.Users.Include(x => x.Chats).FirstOrDefaultAsync(x => x.Id == UserId),
                    Chat = new List<Chat>()
                    {
                        chat
                    }
                };

                await AC.Messages.AddAsync(Mesg);
                await AC.SaveChangesAsync();
                return chat;
            
            
        }
        public async void DelMessage(int mesgId)
        {
            using var AC = new ApplicationContext();

            try
                {
                    AC.Remove(await AC.Messages.FirstOrDefaultAsync(x => x.Id == mesgId));
                    await AC.SaveChangesAsync();
                }
                catch
                {

                }
            
                
            
        }
        public async void DisplayMessage(int mesgId)
        {
            using var AC = new ApplicationContext();

            try
                {
                    Message msg = await AC.Messages.FirstOrDefaultAsync(x => x.Id == mesgId);
                    msg.IsDisplay = true;
                    AC.Update(msg);
                    await AC.SaveChangesAsync();
                }
                catch
                {

                }
            
        }
        public async Task <string> EditMessage(int mesgId,string Text)
        {
            using var AC = new ApplicationContext();

            Message Msg = await AC.Messages.FirstOrDefaultAsync(x => x.Id == mesgId);
                Msg.Text = Text;
                AC.Update(Msg);
                await AC.SaveChangesAsync();
                return "good";
            
        }
       
    }
}
