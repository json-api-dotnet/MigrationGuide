using Example.Api.Resources;
using Microsoft.EntityFrameworkCore;

namespace Example.Api.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Person> People { get; set; }
        public DbSet<Book> Books { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
    }
}
