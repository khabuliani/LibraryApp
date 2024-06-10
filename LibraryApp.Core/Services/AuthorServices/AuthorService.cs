using AutoMapper;
using AutoMapper.Internal.Mappers;
using LibraryApp.Domain.Dto;
using LibraryApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Core.Services.AuthorServices
{
    public class AuthorService : IAuthorService
    {
        private readonly LibraryAppDbContext _db; 
        private readonly IMapper _mapper;
        public AuthorService(LibraryAppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<AuthorDto> CreateAsync(AuthorDto input)
        {
            var data = _mapper.Map<Author>(input);
            _db.Set<Author>().Add(data);
            await _db.SaveChangesAsync();
            input.Id = data.Id;
            return input;
        }
        public async Task<AuthorDto> UpdateAsync(AuthorDto input)
        {
            var data = _db.Set<Author>().FirstOrDefault(x => x.Id == input.Id); 
            if (data == null)
            {
                throw new Exception("Not Found");
            }
            _mapper.Map(input,data);    
            _db.Set<Author>().Update(data);
            await _db.SaveChangesAsync();
            return input;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var data = _db.Set<Author>().FirstOrDefault(x => x.Id == id);
            if (data == null)
            {
                throw new Exception("Not Found");
            }
            _db.Set<Author>().Remove(data);
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<AuthorDto> GetAsync(int id)
        {
            var data = _db.Set<Author>().FirstOrDefault(x => x.Id == id);
            if (data == null)
            {
                throw new Exception("Not Found");
            }
            var result = _mapper.Map<AuthorDto>(data);
            return result;
        }
        public async Task<List<GetAllAuthorDto>> GetAllAsync(GetAllAuthorInputDto input)
        {
            var data = from a in _db.Set<Author>()
                       select new GetAllAuthorDto()
                       { 
                           Id = a.Id,
                           FirstName = a.FirstName,
                           LastName = a.LastName,
                       };
            if(input.Name != null)
            {
                data = data.Where(x => x.FirstName.Contains(input.Name) || x.LastName.Contains(input.Name));
            }
            return data.ToList();
        }
    }
}
