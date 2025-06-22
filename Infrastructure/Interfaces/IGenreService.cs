using Domain.ApiResponse;
using Domain.DTOs.Genre;

namespace Infrastructure.Interfaces;

public interface IGenreService
{
    Task<Response<List<CetGenreDTO>>> GetAllAsync();
    Task<Response<CetGenreDTO>> GetByIdAsync(int id);
    Task<Response<string>> CreateAsync(CreateGenreDTO genre);
    Task<Response<string>> UpdateAsync(int id, UpdateGenreDTO genre);
    Task<Response<string>> DeleteAsync(int id);
}
