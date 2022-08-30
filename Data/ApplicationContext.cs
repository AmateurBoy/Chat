using ChatMarchenkoIlya.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Net.Sockets;
using System.Security.Principal;
using System.Text.Json;

namespace ChatMarchenkoIlya.Data
{
    public class ApplicationContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
        
        public DbSet<Message> Messages { get; set; }
        

        protected readonly IConfiguration Configuration;

        public ApplicationContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public ApplicationContext()
        {
            Database.EnsureCreated();
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            
           
            options.UseSqlServer(Connect.getInstance("d").Name);
            
        }
        
        


    }
}
