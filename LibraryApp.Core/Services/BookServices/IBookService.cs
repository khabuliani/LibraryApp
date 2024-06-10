using LibraryApp.Domain;
using LibraryApp.Domain.Dto;

namespace LibraryApp.Core.Services.BookServices;

public interface IBookService
{
    public Task<BookDto> CreateAsync(BookDto input);
    public Task<BookDto> UpdateAsync(BookDto input);
    public Task<bool> DeleteAsync(int id);
    public Task<bool> TakeAsync(int id);
    public Task<BookDto> GetAsync(int id);
    public Task<List<GetAllBookDto>> GetAllAsync(GetAllBookInputDto input);
}
