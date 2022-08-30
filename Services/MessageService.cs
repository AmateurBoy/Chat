using ChatMarchenkoIlya.Data;
using ChatMarchenkoIlya.Entitys;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ChatMarchenkoIlya.Services
{
    public class MessageService
    {
        static ApplicationContext AC = new ApplicationContext();
        
        public Message GetMessage(int mesgId)
        {
            
                return AC.Messages.Include(x => x.User).FirstOrDefault(x => x.Id == mesgId);
            

        }
        public Chat CreateMessage(string Text, int UserId,int ChatId)
        {
            
                Chat chat = AC.Chats.Include(x => x.Messages).Include(x => x.Users).FirstOrDefault(x => x.Id == ChatId);
                
                Message Mesg = new()
                {
                    Text = Text,
                    dateTime = DateTime.Now,
                    User= AC.Users.Include(x => x.Chats).FirstOrDefault(x => x.Id == UserId),
                    Chat= new List<Chat>() 
                    {
                        chat
                    }
                };  
                
                AC.Messages.Add(Mesg);
                AC.SaveChanges();
                return chat;
            
        }
        public Chat CreateMessageReply(string Text, int UserId, int ChatId)
        {
            
                Chat chat = AC.Chats.Include(x => x.Messages).Include(x => x.Users).FirstOrDefault(x => x.Id == ChatId);

                Message Mesg = new()
                {
                    Text = Text,
                    Reply = true,
                    dateTime = DateTime.Now,
                    User = AC.Users.Include(x => x.Chats).FirstOrDefault(x => x.Id == UserId),
                    Chat = new List<Chat>()
                    {
                        chat
                    }
                };

                AC.Messages.Add(Mesg);
                AC.SaveChanges();
                return chat;
            
        }
        public void DelMessage(int mesgId)
        {
            
                try
                {
                    AC.Remove(AC.Messages.FirstOrDefault(x => x.Id == mesgId));
                    AC.SaveChanges();
                }
                catch
                {

                }
                
            
        }
        public void DisplayMessage(int mesgId)
        {
            
                try
                {
                    Message msg = AC.Messages.FirstOrDefault(x => x.Id == mesgId);
                    msg.IsDisplay = true;
                    AC.Update(msg);
                    AC.SaveChanges();
                }
                catch
                {

                }

            
        }
        public void EditMessage(int mesgId,string Text)
        {
           
                Message Msg = AC.Messages.FirstOrDefault(x => x.Id == mesgId);
                Msg.Text = Text;
                AC.Update(Msg);
                AC.SaveChanges();
            
        }
        public void ReplyMessage(string Text, int UserId, int ChatId)
        {
            using (ApplicationContext AC = new())
            {
                
            }
        }
    }
}
