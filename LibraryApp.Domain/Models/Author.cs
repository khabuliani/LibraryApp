namespace LibraryApp.Domain.Models;

public class Author
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public ICollection<AuthorConnection> Connections { get; set; }
}
