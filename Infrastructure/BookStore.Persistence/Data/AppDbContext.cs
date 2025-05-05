using BookStore.Domain.Entities.Authors;
using BookStore.Domain.Entities.Books;
using BookStore.Domain.Entities.Categories;
using BookStore.Domain.Entities.Reviews;
using BookStore.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Persistence.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<BookCategory> BookCategories { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<BookAuthor> BookAuthors{ get; set; }
    public DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}

