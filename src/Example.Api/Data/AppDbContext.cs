using Example.Api.Resources;
using Microsoft.EntityFrameworkCore;

namespace Example.Api.Data;

public class AppDbContext : DbContext
{
    public DbSet<Person> People => Set<Person>();
    public DbSet<Book> Books => Set<Book>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}
