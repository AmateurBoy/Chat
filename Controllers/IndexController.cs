using ChatMarchenkoIlya.Data;
using ChatMarchenkoIlya.Entitys;
using ChatMarchenkoIlya.Interfaces;
using ChatMarchenkoIlya.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Linq;

namespace ChatMarchenkoIlya.Controllers
{


    [ApiController]
    public class IndexController : Controller
    {
        [HttpGet("/afafasfassagasgagasgasgasgsaga")]
        public User GetCooki()
        {

            User user = new();
            if (Request.Cookies.ContainsKey("User"))
            {
                user = userService.GetUser(int.Parse(Request.Cookies["User"]));
                Response.Cookies.Append("countmsg", $"20");
            }
            else
            {
                user = userService.Registers(user);
                Response.Cookies.Append("User", $"{user.Id}");
            }

            return user;
        }
        [HttpGet("/SetCookiCountMeg")]
        public IActionResult SetCookiCountMeg(int count, int UserId, int ChatId)
        {
            if (Request.Cookies.ContainsKey("countmsg"))
            {
                int a = int.Parse(Request.Cookies["countmsg"]);
                count += a;
                Response.Cookies.Append("countmsg", $"{count}");
            }
            else
                Response.Cookies.Append("countmsg", $"{count}");
            obj obj = new();
            obj.ChatId = ChatId;
            obj.UserId = UserId;
            return RedirectToAction("Message", obj);
        }
        public class obj
        {
            public int ChatId { get; set; }
            public int UserId { get; set; }

        };
        UserService userService = new();
        MessageService messageService = new();
        ChatService chatService = new();



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
            var count = 5;
            if (Request.Cookies.ContainsKey("countmsg"))
            {
                count = int.Parse(Request.Cookies["countmsg"]);
            }

            List<Object> obj = new();
            Chat Ch = chatService.GetChat(ChatId);
            List<Message> C = Ch.Messages.TakeLast(count).OrderBy(x => x.dateTime).ToList();
            Ch.Messages = C;
            obj.Add(Ch);
            obj.Add(UserId);
            return View(@"Pages\Message\MessagersPage.cshtml", obj);
        }

        [HttpGet("/Seending")]
        public IActionResult Seending(string Text, int ChatId, int UserId)
        {
            obj obj = new();
            obj.ChatId = ChatId;
            obj.UserId = UserId;
            messageService.CreateMessage(Text, UserId, ChatId);

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
        public IActionResult ReplyAll(int UserId, int ChatId, int msgId)
        {
            return View(@"Pages/Message/ReplyMessage.cshtml");
        }
        [HttpGet("/ReplyPrivat")]
        public async Task<IActionResult> ReplyPriv(int UserId,int msgId)
        {
            List<Object> objl = new();
            User user = await Task.Run(() => userService.GetUser(UserId));
            Message Msg = await Task.Run(() => messageService.GetMessage(msgId));
            Chat c = await Task.Run(() => chatService.AddChat($"{Msg.User.Name}||{user.Name}", UserId, Msg.User, true));
            if(c.Messages==null)
            {
                c = await Task.Run(() => chatService.AddMessageChat(c, Msg));
            }            
            objl.Add(c);
            objl.Add(UserId);
            return View(@"Pages\Message\MessagersPage.cshtml", objl);
        }
    }
}
