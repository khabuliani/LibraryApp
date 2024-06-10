using LibraryApp.Core.Services.BookServices;
using LibraryApp.Domain;
using LibraryApp.Domain.Dto;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;
    public BookController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpPost]
    public async Task<BookDto> CreateAsync(BookDto input)
    {
        var result = await _bookService.CreateAsync(input);
        return result;
    }

    [HttpPut]
    public async Task<BookDto> UpdateAsync(BookDto input)
    {
        var result = await _bookService.UpdateAsync(input);
        return result;
    }

    [HttpDelete]
    public async Task<bool> DeleteAsync(int id)
    {
        var result = await _bookService.DeleteAsync(id);
        return result;
    }

    [HttpPost]
    public async Task<bool> TakeAsync(int id)
    {
        var result = await _bookService.TakeAsync(id);
        return result;
    }

    [HttpGet]
    public async Task<BookDto> GetAsync(int id)
    {
        var result = await _bookService.GetAsync(id);
        return result;
    }

    [HttpPost]
    public async Task<List<GetAllBookDto>> GetAllAsync(GetAllBookInputDto input)
    {
        var result = await _bookService.GetAllAsync(input);
        return result;
    }
}
