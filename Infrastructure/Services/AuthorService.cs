using System.Net;
using AutoMapper;
using Domain.Entities;
using Domain.ApiResponse;
using Domain.Filter;
using Domain.DTOs;
using Domain.DTOs.Author;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class AuthorService(DataContext context) : IAuthorService
{
    public async Task<PagedResponse<List<GetAuthorDTO>>> GetAllAsync(AuthorFilter filter)
    {
        var validFilter = new ValidFilter(filter.PageNumber, filter.PageSize);
        var authors = context.Authors.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            authors = authors.Where(c => c.Name.ToLower().Trim().Contains(filter.Name.ToLower().Trim()));

        }

        var totalCount = await authors.CountAsync();
        var data = await authors
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .Select(a => new GetAuthorDTO
            {
                Id = a.Id,
                Name = a.Name,
                BirthDate = a.BirthDate,
                Country = a.Country
            }).ToListAsync();

        return new PagedResponse<List<GetAuthorDTO>>(data, totalCount, validFilter.PageNumber, validFilter.PageSize);
    }

    public async Task<Response<GetAuthorDTO>> GetByIdAsync(int id)
    {
        var author = await context.Authors.FindAsync(id);
        if (author == null)
            return new Response<GetAuthorDTO>("Author not found", HttpStatusCode.NotFound);

        var getAuthorDTO = new GetAuthorDTO
        {
            Id = author.Id,
            Name = author.Name,
            BirthDate = author.BirthDate,
            Country = author.Country
        };

        return getAuthorDTO == null
            ? new Response<GetAuthorDTO>("Author not found", HttpStatusCode.NotFound)
            : new Response<GetAuthorDTO>(getAuthorDTO, "Author found");
    }

    public async Task<Response<string>> CreateAsync(CreateAuthorDTO author)
    {
        if (author == null)
            return new Response<string>("Invalid author data", HttpStatusCode.BadRequest);

        var newAuthor = new Author
        {
            Name = author.Name,
            BirthDate = author.BirthDate,
            Country = author.Country
        };

        await context.Authors.AddAsync(newAuthor);
        var result = await context.SaveChangesAsync();
        return result > 0
            ? new Response<string>("Author created successfully")
            : new Response<string>("Author not created", HttpStatusCode.InternalServerError);
    }

    public async Task<Response<string>> UpdateAsync(int id, UpdateAuthorDTO author)
    {
        var updateAuthor = await context.Authors.FindAsync(id);
        if (updateAuthor == null)
            return new Response<string>("Author not found", HttpStatusCode.NotFound);

        updateAuthor.Name = author.Name;
        updateAuthor.BirthDate = author.BirthDate;
        updateAuthor.Country = author.Country;

        var result = await context.SaveChangesAsync();
        return result > 0
            ? new Response<string>("Author not updated", HttpStatusCode.NotFound)
            : new Response<string>(null, "Author updated");
    }

    public async Task<Response<string>> DeleteAsync(int id)
    {
        var author = await context.Authors.FindAsync(id);
        if (author == null)
            return new Response<string>("Author not found", HttpStatusCode.NotFound);

        context.Authors.Remove(author);
        var result = await context.SaveChangesAsync();
        return result > 0
            ? new Response<string>("Author not deleted", HttpStatusCode.InternalServerError)
            : new Response<string>(null, "Author deleted");
    }

}
