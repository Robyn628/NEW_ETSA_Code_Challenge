using Abp.Authorization.Users;
using Code_Challenge.Models.UserModel;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Code_Challenge.DB
{
    public class UserDBContext : DbContext
    {
        public UserDBContext()
        {
        }

        public UserDBContext(DbContextOptions options) : base(options)
        { }

        public virtual DbSet<User> Users { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TestDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Username)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        private void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            throw new NotImplementedException();
        }
    }
}
