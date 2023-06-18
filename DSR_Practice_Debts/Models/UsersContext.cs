using Microsoft.EntityFrameworkCore;

namespace DSR_Practice_Debts.Models
{
    public class UsersContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Debt> Debts { get; set; }

        public UsersContext(DbContextOptions<UsersContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
