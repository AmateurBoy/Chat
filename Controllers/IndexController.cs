using ChatMarchenkoIlya.Data;
using ChatMarchenkoIlya.Entitys;
using ChatMarchenkoIlya.Interfaces;
using ChatMarchenkoIlya.Services;

using Microsoft.AspNetCore.Mvc;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace ChatMarchenkoIlya.Controllers
{

    [ApiController]
    public class IndexController : Controller
    {
        public  class obj 
        {
            public  int ChatId { get; set; }
            public  int UserId { get; set; }
    
        };
        UserService userService=new();
        MessageService messageService = new();
        ChatService chatService = new();

        
        

        [HttpGet("/")]
        public IActionResult Index()
        {
             User GetCooki()
            {
                User user = new();
                if (Request.Cookies.ContainsKey("User"))
                {
                    user = userService.GetUser(int.Parse(Request.Cookies["User"]));

                }
                else
                {
                    user = userService.Registers(user);
                    Response.Cookies.Append("User", $"{user.Id}");
                }
                
                return user;
            }
            List<Object> obj = new();
            obj.Add(GetCooki());
            obj.Add(GetCooki().Id);
            return View(@"Pages\Index.cshtml",obj);
        }
        
        [HttpGet("/Message")]
        public async Task<IActionResult> Message(int ChatId, int UserId)
        {
            List<Object> obj = new();
            Chat Ch = chatService.GetChat(ChatId);
            List<Message> C = Ch.Messages.OrderBy(x=>x.dateTime).ToList();
            Ch.Messages = C;
            obj.Add(Ch);
            obj.Add(UserId);
            return View(@"Pages\Message\MessagersPage.cshtml",obj);
        }
        
        [HttpGet("/Seending")]
        
        public IActionResult Seending(string Text,int ChatId,int UserId)
        {
            obj obj = new();          
            obj.ChatId=ChatId;
            obj.UserId=UserId;
            messageService.CreateMessage(Text, UserId, ChatId);
            
            return RedirectToAction("Message",obj); 
        }
        [HttpGet("/CreateChat")]
        public IActionResult CreateChat(int UserId)
        {
            List<Object> obj = new();
            obj.Add(0);
            obj.Add(UserId);
            return View(@"Pages/ChatWork/ChatCreate.cshtml",obj);
        }
        [HttpGet("/SelectChekChat")]
        public IActionResult SelectChekChat(int UserId)
        {
            List<Object> obj = new();
            obj.Add(userService.GetUser(UserId));
            obj.Add(UserId);
            return View(@"Pages/ChatWork/AddUserChat.cshtml",obj);
        }
        [HttpGet("/AddUserInChat")]
        
        public IActionResult AddUserInChat(string UserName,int UserId,int ChatId)
        {

            List<Object> obj = new();
            obj.Add(userService.GetUser(UserId));
            obj.Add(UserId);
            obj.Add(chatService.ConnectChat(ChatId, UserName));
            return View(@"Pages/Index.cshtml",obj);
        }
        [HttpGet("/AddChat")]
        public IActionResult AddChat(string NameChat, int UserID)
        {
            List<Object> obj = new();
            bool IsPrivate = false;
            if (Request.Query.FirstOrDefault(x => x.Key == "IsPrivate").Value=="on")
            {
                IsPrivate = true;
            }
           
            obj.Add(chatService.AddChat(NameChat, UserID,IsPrivate));
            obj.Add(UserID);
            return View(@"Pages\Message\MessagersPage.cshtml", obj);
        }
        
        [HttpGet("/Chat")]
        public IActionResult Chat(int UserID)
        {            
            return Json(chatService.GetChatUser(userService.GetUser(UserID)));
        }
        [HttpGet("/ExitChat")]
        public IActionResult ExitChat(int UserID,int ChatID)
        {
            
            
            return Json(chatService.ExitChat(userService.GetUser(UserID),ChatID));
        }
    }
}
