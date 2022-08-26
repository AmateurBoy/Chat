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
        [HttpGet("/ConnectChat")]
        public IActionResult CommectChat(int ChatID, int UserID)
        {
            return Json(chatService.ConnectChat(ChatID, userService.GetUser(UserID)));
        }
        [HttpGet("/Message")]
        public async Task<IActionResult> Message(int ChatId,int UserId)
        {
            List<Object> obj = new();
            obj.Add(chatService.GetChat(ChatId));
            obj.Add(UserId);
            return View(@"Pages\Message\MessagersPage.cshtml",obj);
        }
        [HttpGet("/Seending")]
        public IActionResult Seending(int ChatId, int UserId, string Text)
        {
            List<Object> obj = new();
            obj.Add(messageService.CreateMessage(Text, UserId, ChatId));
            obj.Add(UserId);
            return View(@"Pages\Message\MessagersPage.cshtml", obj); 
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
        public IActionResult SelectChekChat(int ChatId, int UserId)
        {
            return View(@"Pages/Index.cshtml");
        }
            [HttpGet("/AddUserInChat")]
        public IActionResult AddUserInChat(int ChatId, int UserId)
        {
            List<Object> obj = new();
            obj.Add(0);
            obj.Add(UserId);
            obj.Add(chatService.ConnectChat(ChatId, UserId));
            return View(@"Pages/Index.cshtml", obj);
        }
        [HttpGet("/AddChat")]
        public IActionResult AddChat(string NameChat, int UserID)
        {
            List<Object> obj = new();
            obj.Add(chatService.AddChat(NameChat, UserID));
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
