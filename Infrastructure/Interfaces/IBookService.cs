using Domain.ApiResponse;
using Domain.DTOs;
using Domain.DTOs.Book;
using Domain.Filter;

namespace Infrastructure.Interfaces;

public interface IBookService
{
    Task<PagedResponse<List<GetBookDTO>>> GetAllAsync(BookFilter filter);
    Task<Response<GetBookDTO>> GetByIdAsync(int id);
    Task<Response<string>> CreateAsync(CreateBookDTO createBookDTO);
    Task<Response<string>> UpdateAsync(int id,UpdateBookDTO updateBookDTO);
    Task<Response<string>> DeleteAsync(int id);
}
