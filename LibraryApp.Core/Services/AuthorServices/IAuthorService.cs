using LibraryApp.Domain.Dto;

namespace LibraryApp.Core.Services.AuthorServices;

public interface IAuthorService
{
    public  Task<AuthorDto> CreateAsync(AuthorDto input);
    public  Task<AuthorDto> UpdateAsync(AuthorDto input);
    public  Task<bool> DeleteAsync(int id);
    public Task<AuthorDto> GetAsync(int id);
    public Task<List<GetAllAuthorDto>> GetAllAsync(GetAllAuthorInputDto input);
}
