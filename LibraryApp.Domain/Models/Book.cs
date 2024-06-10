namespace LibraryApp.Domain.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Picture { get; set; }
    public float Rate { get; set; }
    public DateTime CreationDate { get; set; }
    public bool IsTaken { get; set; }
    public ICollection<AuthorConnection> Connections { get; set; }
}
