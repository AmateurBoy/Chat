using ChatMarchenkoIlya.Data;
using ChatMarchenkoIlya.Entitys;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace ChatMarchenkoIlya.Services
{
    
    public  class UserService
    {
        

        public User Registers(User user)
        {
            using (ApplicationContext AC = new())
            {
                Random random = new Random();
                if (AC.Users.FirstOrDefault(x => x.Id == user.Id) == null)
                {
                    user.Name = $"User{random.Next(000000, 999999)}";
                    Chat findchat = AC.Chats.Include(x => x.Messages).Include(x => x.Users).FirstOrDefault(x => x.Id == 29);
                    findchat.Users.Add(user);
                    AC.Users.Add(user);
                    AC.Chats.Update(findchat);                    
                    AC.SaveChanges();
                    return user;
                }
                else
                {
                    throw new System.ArgumentException("Акаунт уже существует", nameof(user));
                }
            }
        }
        public List<User> GetAllUsers()
        {
            using (ApplicationContext AC = new())
            {
                List<User> users = AC.Users.ToList<User>();
                return users;
            }
        }
        public  User GetUser(int id)
        {
            using (ApplicationContext AC = new())
            {
                
                User user = AC.Users.Include(x => x.Chats).FirstOrDefault(x=>x.Id==id);
                return user;                
            }
        }
     
        public void EditUser(int id,string newName)
        {
            using (ApplicationContext AC = new())
            {
                User U = AC.Users.FirstOrDefault(x => x.Id == id);
                U.Name = newName;
                AC.Users.Update(U);
                AC.SaveChanges();
            }
        }

    }
}
