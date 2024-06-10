using AutoMapper;
using LibraryApp.Domain.Dto;
using LibraryApp.Domain.Models;

namespace LibraryApp.Core.Services.BookServices;
public class BookService : IBookService
{
    private readonly LibraryAppDbContext _db;
    private readonly IMapper _mapper;
    public BookService(LibraryAppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }
    public async Task<BookDto> CreateAsync(BookDto input)
    {
        var data = _mapper.Map<Book>(input);
        _db.Set<Book>().Add(data);
        await _db.SaveChangesAsync();
        input.Id = data.Id;
        return input;
    }
    public async Task<BookDto> UpdateAsync(BookDto input)
    {
        var data = _db.Set<Book>().FirstOrDefault(x => x.Id == input.Id);
        if (data == null)
        {
            throw new Exception("Not Found");
        }
        var toRemove = _db.Set<AuthorConnection>().Where(x => !input.Connections.Select(a => a.Id).Contains(input.Id)).ToList();
        _db.Set<AuthorConnection>().RemoveRange(toRemove);
        _mapper.Map(input, data);
        _db.Set<Book>().Update(data);
        await _db.SaveChangesAsync();
        return input;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var data = _db.Set<Book>().FirstOrDefault(x => x.Id == id);
        if (data == null)
        {
            throw new Exception("Not Found");
        }
        _db.Set<Book>().Remove(data);
        await _db.SaveChangesAsync();
        return true;
    }
    public async Task<bool> TakeAsync(int id)
    {
        var data = _db.Set<Book>().FirstOrDefault(x => x.Id == id); 
        if (data == null)
        {
            throw new Exception("Not Found");
        }
        if (data.IsTaken == false)
        {
            data.IsTaken = true;
        }
        else
        {
            data.IsTaken = false;
        }
        _db.Set<Book>().Update(data);
        await _db.SaveChangesAsync();
        return true;
    }
    public async Task<BookDto> GetAsync(int id)
    {
        var data = _db.Set<Book>().FirstOrDefault(x => x.Id == id);
        if(data == null)
        {
            throw new Exception("Not Found");
        }
        var result = _mapper.Map<BookDto>(data);
        var authors = _db.Set<Author>().Where(x => x.Connections.Any(a => a.BookId == id)).ToList();
        result.Connections = _mapper.Map <List<AuthorConnectionDto>>(_db.Set<AuthorConnection>().Where(x => x.BookId == id).ToList());
        var authorList = authors.Select(x => x.FirstName + " " + x.LastName).ToList();
        result.Authors = string.Join(", ", authorList);
        return result;
    }
    public async Task<List<GetAllBookDto>> GetAllAsync(GetAllBookInputDto input)
    {
        var data = from a in _db.Set<Book>()
                   select new GetAllBookDto()
                   {
                       Id = a.Id,
                       Title = a.Title,
                       Picture = a.Picture,
                       Description = a.Description
                   };
        if (input.Title != null)
        {
            data = data.Where(x => x.Title.Contains(input.Title));
        }
        return data.ToList();
    }
}

