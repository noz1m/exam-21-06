using Domain.ApiResponse;
using Domain.DTOs;
using Domain.DTOs.Author;
using Domain.Filter;

namespace Infrastructure.Interfaces;

public interface IAuthorService
{
    Task<PagedResponse<List<GetAuthorDTO>>> GetAllAsync(AuthorFilter filter);
    Task<Response<GetAuthorDTO>> GetByIdAsync(int id);
    Task<Response<string>> CreateAsync(CreateAuthorDTO author);
    Task<Response<string>> UpdateAsync(int id,UpdateAuthorDTO author);
    Task<Response<string>> DeleteAsync(int id);
}
