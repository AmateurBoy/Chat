using ChatMarchenkoIlya.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Net.Sockets;
using System.Security.Principal;


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
            options.UseSqlServer("Data Source=chatdbtest.database.windows.net,1433;Initial Catalog=coreDb;User ID=superuser;Password=Admin159753;");
            
        }
        
        


    }
}
