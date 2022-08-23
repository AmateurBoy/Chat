using ChatMarchenkoIlya.Entitys;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;
using System.Security.Principal;

namespace ChatMarchenkoIlya.Data
{
    public class ApplicationContext: DbContext
    {
        private static string IpServer = "(localdb)";
        private static string NameDB = "KinoDataBase";
        public DbSet<User> Accounts { get; set; }
        public DbSet<Chat> Billings { get; set; }
        public DbSet<Message> Movies { get; set; }
        
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }
    }
}
