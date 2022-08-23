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
        
        [HttpGet("/users")]        
        public IActionResult AllUsers()
        {
            
            Console.WriteLine("Подключение к Базе Данних");
            
            return Json(userService.GetAllUsers());
        }
        [HttpGet("/Chat")]
        public IActionResult Chat(string NameChat,int UserID )
        {            
            Console.WriteLine("Подключение к Базе Данних");
            chatService.AddChat(NameChat, userService.GetUser(UserID));
            return Json(chatService.GetChatUser(userService.GetUser(UserID)));
        }
    }
}
