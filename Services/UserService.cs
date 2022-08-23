using ChatMarchenkoIlya.Data;
using ChatMarchenkoIlya.Entitys;
using ChatMarchenkoIlya.Interfaces;

namespace ChatMarchenkoIlya.Services
{
    public class UserService
    {
        public User Registers(User user)
        {
            using (ApplicationContext AC = new())
            {
                if (AC.Users.FirstOrDefault(x => x.Id == user.Id) == null)
                {
                    AC.Users.Add(user);
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
        public User GetUser(int id)
        {
            using (ApplicationContext AC = new())
            {
                User user = AC.Users.FirstOrDefault(x => x.Id == id);
                return user;                
            }
        }

    }
}
