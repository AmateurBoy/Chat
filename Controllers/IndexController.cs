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
        //[HTTPGET("/CONNECTCHAT")]
        //PUBLIC IACTIONRESULT COMMECTCHAT(INT CHATID, INT USERID)
        //{
        //    //RETURN JSON(CHATSERVICE.CONNECTCHAT(CHATID, USERID));
        //}
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
