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
    }
}
