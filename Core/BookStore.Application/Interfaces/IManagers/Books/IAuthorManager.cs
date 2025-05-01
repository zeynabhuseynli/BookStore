using BookStore.Application.DTOs.AuthorDtos;
using BookStore.Domain.Entities.Authors;

namespace BookStore.Application.Interfaces.IManagers.Books;
public interface IAuthorManager
{
    Task<IEnumerable<AuthorDto>> GetAllAsync();
    Task<AuthorDto?> GetByIdAsync(int id);
    Task<bool> CreateAsync(CreateAuthorDto dto);
    Task<bool> UpdateAsync(UpdateAuthorDto dto);
}

