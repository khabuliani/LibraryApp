using LibraryApp.Core.Services.AuthorServices;
using LibraryApp.Domain.Dto;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthorController : ControllerBase
{
    private readonly IAuthorService _authorService;
    public AuthorController(
        IAuthorService authorService)
    {
        _authorService = authorService;
    }

    [HttpGet]
    public async Task<AuthorDto> GetAsync(int id)
    {
        var result = await _authorService.GetAsync(id);
        return result;
    }

    [HttpPost]
    public async Task<AuthorDto> CreateAsync(AuthorDto input)
    {
        var result = await _authorService.CreateAsync(input);
        return result;
    }

    [HttpPut]
    public async Task<AuthorDto> UpdateAsync(AuthorDto input)
    {
        var result = await _authorService.UpdateAsync(input);
        return result;
    }

    [HttpDelete]
    public async Task<bool> DeleteAsync(int id)
    {
        var result = await _authorService.DeleteAsync(id);
        return result;
    }

    [HttpPost]
    public async Task<List<GetAllAuthorDto>> GetAllAsync(GetAllAuthorInputDto input)
    {
        var result = await _authorService.GetAllAsync(input);
        return result;
    }
}
