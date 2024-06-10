namespace LibraryApp.Domain.Models;

public class AuthorConnection
{
    public int Id { get; set; } 
    public int BookId { get; set; }
    public Book Book { get; set; }
    public int AuthorId { get; set; }
    public Author Author { get; set; }
}
