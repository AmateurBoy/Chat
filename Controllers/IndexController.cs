using ChatMarchenkoIlya.Entitys;
using ChatMarchenkoIlya.Interfaces;
using ChatMarchenkoIlya.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChatMarchenkoIlya.Controllers
{
    [ApiController]
    public class IndexController : Controller
    {
        Random R = new();
        UserService userService=new();
        MessageService messageService = new();
        ChatService chatService = new();
        
        [HttpGet("/RegUsers")]        
        public IActionResult AllUsers()
        {            
            Console.WriteLine("Подключение к Базе Данних");
            User user = new()
            {
                Name = $"User{R.Next(000000, 999999)}",
            };
            userService.Registers(user);
            return Json(userService.GetAllUsers());
        }
        [HttpGet("/ConnectChat")]
        public IActionResult AddChat(int ChatID, int UserID)
        {
            Console.WriteLine("Подключение к Базе Данних");
            return Json(chatService.ConnectChat(ChatID, userService.GetUser(UserID)));
        }
        [HttpGet("/AddChat")]
        public IActionResult AddChat(string NameChat, int UserID)
        {
            Console.WriteLine("Подключение к Базе Данних");
            
            return Json(chatService.AddChat(NameChat, userService.GetUser(UserID)));
        }
        [HttpGet("/Chat")]
        public IActionResult Chat(int UserID)
        {            
            Console.WriteLine("Подключение к Базе Данних");
           
            return Json(chatService.GetChatUser(userService.GetUser(UserID)));
        }
        [HttpGet("/ExitChat")]
        public IActionResult ExitChat(int UserID,int ChatID)
        {
            Console.WriteLine("Подключение к Базе Данних");
            
            return Json(chatService.ExitChat(userService.GetUser(UserID),ChatID));
        }
    }
}
