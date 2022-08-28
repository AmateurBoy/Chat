using ChatMarchenkoIlya.Data;
using ChatMarchenkoIlya.Entitys;
using ChatMarchenkoIlya.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ChatMarchenkoIlya.Services
{
    public class MessageService
    {
        public Message GetMessage(int mesgId)
        {
            using (ApplicationContext AC = new())
            {
                return AC.Messages.Include(x => x.User).FirstOrDefault(x => x.Id == mesgId);
            }

        }
        public Chat CreateMessage(string Text, int UserId,int ChatId)
        {
            using (ApplicationContext AC = new())
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
        }
        public void DelMessage(int mesgId)
        {
            using (ApplicationContext AC = new())
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
        }
        public void DisplayMessage(int mesgId)
        {
            using (ApplicationContext AC = new())
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
        }
        public void EditMessage(int mesgId,string Text)
        {
            using (ApplicationContext AC = new())
            {
                Message Msg = AC.Messages.FirstOrDefault(x => x.Id == mesgId);
                Msg.Text = Text;
                AC.Update(Msg);
                AC.SaveChanges();
            }
        }
        public void ReplyMessage(string Text, int UserId, int ChatId)
        {
            using (ApplicationContext AC = new())
            {
                
            }
        }
    }
}
