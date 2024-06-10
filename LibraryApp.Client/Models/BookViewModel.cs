using LibraryApp.Domain.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryApp.Client.Models;

public class BookViewModel
{
    public BookDto Book { get; set; }
    public List<SelectListItem> Authors { get; set; }
    public List<int> SelectedAuthorIds { get; set; }
}
