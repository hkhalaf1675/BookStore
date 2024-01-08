using DB_Assigment.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DB_Assigment.Contexts
{
    public class ApplicationDbContext:IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions options):base(options)
        {}
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // make the book code primary key
            builder.Entity<Book>().HasKey(B => B.Code);

            base.OnModelCreating(builder);
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<BorrowingHistory> BorrowingHistories { get; set; }
    }
}
