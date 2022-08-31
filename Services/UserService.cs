using ChatMarchenkoIlya.Data;
using ChatMarchenkoIlya.Entitys;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace ChatMarchenkoIlya.Services
{
    
    public  class UserService
    {
        

        public async Task<User> Registers(User user)
        {
            using var AC = new ApplicationContext();

            Random random = new Random();
                if (AC.Users.FirstOrDefaultAsync(x => x.Id == user.Id) == null)
                {
                    user.Name = $"User{random.Next(000000, 999999)}";
                    Chat findchat = await AC.Chats.Include(x => x.Messages).Include(x => x.Users).FirstOrDefaultAsync(x => x.Id == 29);
                    findchat.Users.Add(user);
                    await AC.Users.AddAsync(user);
                    AC.Chats.Update(findchat);
                    await AC.SaveChangesAsync();
                    return user;
                }
                else
                {
                    throw new System.ArgumentException("Акаунт уже существует", nameof(user));
                }
            
            
        }
        public async Task<List<User>> GetAllUsers()
        {
            using var AC = new ApplicationContext();

            List<User> users = await AC.Users.ToListAsync<User>();
                return users;
            
        }
        public async Task<User> GetUser(int id)
        {
            using var AC = new ApplicationContext();

            User user = await AC.Users.Include(x => x.Chats).FirstOrDefaultAsync(x => x.Id == id);
                return user;
            
        }     
        public async void EditUser(int id,string newName)
        {
            using var AC = new ApplicationContext();

            User U = await AC.Users.FirstOrDefaultAsync(x => x.Id == id);
                U.Name = newName;
                AC.Users.Update(U);
                await AC.SaveChangesAsync();
            
        }

    }
}
