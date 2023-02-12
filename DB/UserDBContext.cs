using Code_Challenge.Models.UserModel;
using Microsoft.EntityFrameworkCore;

namespace Code_Challenge.DB
{
    public class UserDBContext : DbContext
    {
        public UserDBContext(DbContextOptions options) : base(options)
        {        }

        public DbSet<User> Users { get; set; }
    }
}
