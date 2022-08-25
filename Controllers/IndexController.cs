using ChatMarchenkoIlya.Data;
using ChatMarchenkoIlya.Entitys;
using ChatMarchenkoIlya.Interfaces;
using ChatMarchenkoIlya.Services;

using Microsoft.AspNetCore.Mvc;

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
        public IActionResult AddChat(int ChatID, int UserID)
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
        
        [HttpGet("/AddChat")]
        public IActionResult AddChat(string NameChat, int UserID)
        {
            return Json(chatService.AddChat(NameChat, userService.GetUser(UserID)));
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
