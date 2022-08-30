using ChatMarchenkoIlya.Data;
using ChatMarchenkoIlya.Entitys;

using ChatMarchenkoIlya.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Linq;
using static ChatMarchenkoIlya.Controllers.IndexController;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;

namespace ChatMarchenkoIlya.Controllers
{


    [ApiController]
    public class IndexController : Controller
    {
        public class obj
        {
            public int ChatId { get; set; }
            public int UserId { get; set; }

        };
        UserService userService = new();
        MessageService messageService = new();
        ChatService chatService = new();

        [HttpGet("/NotRequested")]
        public User GetCooki()
        {
            Response.Cookies.Append("minLimit", $"0");
            Response.Cookies.Append("maxLimit", $"21");
            User user = new();
            if (Request.Cookies.ContainsKey("User"))
            {
                user = userService.GetUser(int.Parse(Request.Cookies["User"]));
                if(user==null)
                {
                    user = new User();
                    user.Id = 0;
                    user = userService.Registers(user);
                    Response.Cookies.Append("User", $"{user.Id}");
                }
                
            }
            else
            {
                user = userService.Registers(user);
                Response.Cookies.Append("User", $"{user.Id}");
            }

            return user;
        }
        [HttpGet("/UpdateCount")]
        public IActionResult SetCookiCountMeg(string IsUp,int UserId, int ChatId)
        {
            int maxLimitcount = int.Parse(Request.Cookies["maxLimit"]);            
            int minLimitcount = int.Parse(Request.Cookies["minLimit"]);            
            int count = 21;
            int maxcount = chatService.GetChat(ChatId).Messages.Count;
            if(IsUp=="Yes")
                {
                    if(minLimitcount-count > 0)
                    {
                        minLimitcount -= count;
                        maxLimitcount -= count;
                    }
                    else
                    {
                        minLimitcount = 0;
                        maxLimitcount = 21;
                    }
                }
            else
                {
                    if((maxLimitcount+count)<maxcount)
                    {
                        maxLimitcount += count;
                        minLimitcount += count;
                    }
                    else
                    {
                        maxLimitcount = maxcount;
                        minLimitcount = maxcount - 21;
                    }    
                }
            Response.Cookies.Append("maxLimit", $"{maxLimitcount}");
            Response.Cookies.Append("minLimit", $"{minLimitcount}");
            obj obj = new();
            obj.ChatId = ChatId;
            obj.UserId = UserId;
            return RedirectToAction("Message", obj);
            
        }
        


        [HttpGet("/")]
        public IActionResult Index()
        {
            
            List<Object> obj = new();
            obj.Add(GetCooki());
            obj.Add(GetCooki().Id);
            return View(@"Pages\Index.cshtml", obj);
        }

        [HttpGet("/Message")]
        public IActionResult Message(int ChatId, int UserId)
        {
            var max = 0;
            var min = 21;
            if (Request.Cookies.ContainsKey("minLimit"))
            {
                min = int.Parse(Request.Cookies["minLimit"]);
            }
            if (Request.Cookies.ContainsKey("maxLimit"))
            {
                max = int.Parse(Request.Cookies["maxLimit"]);
            }

            List<Object> obj = new();
            Chat Ch = chatService.GetChat(ChatId);
            List<Message> C = Ch.Messages.Where((o, index) => index >= min & index <= max).OrderBy(x => x.dateTime).ToList();
            Ch.Messages = C;
            obj.Add(Ch);
            obj.Add(UserId);
            return View(@"Pages\Message\MessagersPage.cshtml", obj);
        }

        [HttpGet("/Seending")]
        public IActionResult Seending(string Text, int ChatId, int UserId)
        {
            var max = chatService.GetChat(ChatId).Messages.Count+1;
            var min = 0;
            if (max - 21>=0)
            {
                min = max - 21;
            }
            else
            {
                min = 0;
            }
            obj obj = new();
            obj.ChatId = ChatId;
            obj.UserId = UserId;
            messageService.CreateMessage(Text, UserId, ChatId);
            Response.Cookies.Append("maxLimit", $"{max}");
            Response.Cookies.Append("minLimit", $"{min}");
            return RedirectToAction("Message", obj);
        }
        [HttpGet("/CreateChat")]
        public IActionResult CreateChat(int UserId)
        {
            List<Object> obj = new();
            obj.Add(0);
            obj.Add(UserId);
            return View(@"Pages/ChatWork/ChatCreate.cshtml", obj);
        }
        [HttpGet("/SelectChekChat")]
        public IActionResult SelectChekChat(int UserId)
        {
            List<Object> obj = new();
            obj.Add(userService.GetUser(UserId));
            obj.Add(UserId);
            return View(@"Pages/ChatWork/AddUserChat.cshtml", obj);
        }
        [HttpGet("/AddUserInChat")]
        public IActionResult AddUserInChat(string UserName, int UserId, int ChatId)
        {
            List<Object> obj = new();
            obj.Add(userService.GetUser(UserId));
            obj.Add(UserId);
            obj.Add(chatService.ConnectChat(ChatId, UserName));
            return View(@"Pages/Index.cshtml", obj);
        }
        [HttpGet("/AddChat")]
        public IActionResult AddChat(string NameChat, int UserID)
        {
            List<Object> obj = new();
            bool IsPrivate = false;
            if (Request.Query.FirstOrDefault(x => x.Key == "IsPrivate").Value == "on")
            {
                IsPrivate = true;
            }

            obj.Add(chatService.AddChat(NameChat, UserID, IsPrivate));
            obj.Add(UserID);
            return View(@"Pages\Message\MessagersPage.cshtml", obj);
        }
        [HttpGet("/PageEditMessage")]
        public IActionResult PageEditMessage(int UserId, int ChatId, int MsgId, string Text)
        {
            List<Object> obj = new();
            obj.Add(userService.GetUser(UserId));
            obj.Add(UserId);
            obj.Add(MsgId);
            obj.Add(Text);
            obj.Add(ChatId);

            return View(@"Pages\Message\EditMessage.cshtml", obj);
        }
        [HttpGet("/EditMessage")]
        public IActionResult EditMessage(int UserId, int ChatId, int MsgId, string Text)
        {
            obj obj = new();
            obj.UserId = UserId;
            obj.ChatId = ChatId;
            messageService.EditMessage(MsgId, Text);
            return RedirectToAction("Message", obj);
        }
        [HttpGet("/ExitChat")]
        public IActionResult ExitChat(int UserID, int ChatID)
        {
            chatService.ExitChat(userService.GetUser(UserID), ChatID);
            List<Object> obj = new();
            obj.Add(userService.GetUser(UserID));
            obj.Add(UserID);
            return View(@"Pages/Index.cshtml", obj);
        }
        [HttpGet("/DelMe")]
        public IActionResult DelMe(int UserId, int ChatId, int msgId)
        {
            obj obj = new();
            obj.UserId = UserId;
            obj.ChatId = ChatId;
            messageService.DisplayMessage(msgId);
            return RedirectToAction("Message", obj);
        }
        [HttpGet("/DelAll")]
        public IActionResult DelAll(int UserId, int ChatId, int msgId)
        {
            obj obj = new();
            obj.UserId = UserId;
            obj.ChatId = ChatId;
            messageService.DelMessage(msgId);
            return RedirectToAction("Message", obj);
        }
        [HttpGet("/ReplyPublic")]
        public async Task<IActionResult> ReplyPublic(int UserId, int ChatId, int msgId)
        {
            List<Object> obj = new();
            
            Chat chat = await Task.Run(() => chatService.GetChat(ChatId));
            Message Msg = await Task.Run(() => messageService.GetMessage(msgId));
            
            obj.Add(chat);
            obj.Add(UserId);
            obj.Add(Msg);           
            return View(@"Pages/Message/ReplyMessage.cshtml",obj);
        }
        [HttpGet("/ReplyPublicResult")]
        public async Task<IActionResult> ReplyPublicResult(int UserId, int ChatId, int msgId, string Text)
        {
            string Temp = "";
            obj obj = new();
            User user = await Task.Run(() => userService.GetUser(UserId));
            Message Msg = await Task.Run(() => messageService.GetMessage(msgId));
            Temp = Msg.Text;                        
            Msg.Text = $"{Temp}==>{Text}";
            obj.ChatId=ChatId;
            obj.UserId = UserId;
            await Task.Run(() => messageService.CreateMessageReply(Msg.Text, UserId, ChatId));
            
            return RedirectToAction(@"Message", obj);
        }
        [HttpGet("/ReplyPrivat")]
        public async Task<IActionResult> ReplyPrivate(int UserId,int msgId)
        {
            List<Object> objl = new();
            
            Chat c = new Chat();            
            
            using (ApplicationContext AC = new ApplicationContext())
            {
                User user = AC.Users.Include(x => x.Chats).FirstOrDefault(x => x.Id == UserId); 
                Message Msg = AC.Messages.Include(x => x.User).FirstOrDefault(x => x.Id == msgId);

                c.Name = $"{Msg.User.Name}||{user.Name}";
                c.IsPrivate = true;
                c.Users = new List<User>();
                c.Users.Add(user);
                c.Users.Add(Msg.User);
                if(AC.Chats.FirstOrDefault(x=>x.Name==c.Name)==null)
                {
                    AC.Chats.Add(c);
                    AC.SaveChanges();
                }
                if (c.Messages == null)
                {
                    c.Messages = new List<Message>();
                    c.Messages.Add(Msg);
                    AC.Update(c);
                    AC.SaveChanges();
                }

            }          

                       
            objl.Add(c);
            objl.Add(UserId);
            return View(@"Pages\Message\MessagersPage.cshtml", objl);
        }
    }
}
