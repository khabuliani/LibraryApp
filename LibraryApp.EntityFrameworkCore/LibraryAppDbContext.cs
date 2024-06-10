using LibraryApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

public class LibraryAppDbContext : DbContext
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<AuthorConnection> AuthorConnections { get; set; }
    public LibraryAppDbContext(DbContextOptions<LibraryAppDbContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(a =>
        {
            a.ToTable("Books", "LibraryDB");
            a.Property(x => x.Title).IsRequired().HasMaxLength(120);
            a.Property(x => x.CreationDate).IsRequired();
            a.Property(x => x.Description).IsRequired().HasMaxLength(5000);
            a.Property(x => x.Picture).IsRequired().HasMaxLength(5000);
            a.HasMany(x => x.Connections).WithOne(x => x.Book).HasForeignKey(x => x.BookId);
        }); 
        modelBuilder.Entity<Author>(a =>
        {
            a.ToTable("Authors", "LibraryDB");
            a.Property(x => x.FirstName).IsRequired().HasMaxLength(120);
            a.Property(x => x.LastName).IsRequired().HasMaxLength(120);
            a.Property(x => x.BirthDate).IsRequired();
            a.HasMany(x => x.Connections).WithOne(x => x.Author).HasForeignKey(x => x.AuthorId);
        }); 
        modelBuilder.Entity<AuthorConnection>(a =>
        {
            a.ToTable("AuthorConnections", "LibraryDB");
            a.HasOne(x => x.Author).WithMany(x => x.Connections).HasForeignKey(x => x.AuthorId);
            a.HasOne(x => x.Book).WithMany(x => x.Connections).HasForeignKey(x => x.BookId);
        }); 
        base.OnModelCreating(modelBuilder);
    }
}
