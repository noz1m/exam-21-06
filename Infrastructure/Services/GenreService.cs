using AutoMapper;
using System.Net;
using Domain.Entities;
using Domain.ApiResponse;
using Domain.DTOs;
using Domain.DTOs.Genre;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class GenreService(DataContext context) : IGenreService
{
    public async Task<Response<List<CetGenreDTO>>> GetAllAsync()
    {
        var genre = await context.Genres
            .Select(g => new CetGenreDTO
            {
                Id = g.Id,
                Name = g.Name
            }).ToListAsync();

        return new Response<List<CetGenreDTO>>("Created successfully", HttpStatusCode.OK);
    }

    public async Task<Response<CetGenreDTO>> GetByIdAsync(int id)
    {
        var genre = await context.Genres.FindAsync(id);
        if (genre == null)
            return new Response<CetGenreDTO>("Genre not found", HttpStatusCode.NotFound);

        var genreDTO = new CetGenreDTO
        {
            Id = genre.Id,
            Name = genre.Name
        };

        return genreDTO == null
            ? new Response<CetGenreDTO>("Genre not found", HttpStatusCode.NotFound)
            : new Response<CetGenreDTO>(genreDTO, "Genre found");
    }
    public async Task<Response<string>> CreateAsync(CreateGenreDTO genre)
    {
        if (genre == null)
            return new Response<string>("Invalid genre data", HttpStatusCode.BadRequest);
        
        var newGenre = new Genre
        {
            Name = genre.Name
        };
        
        await context.Genres.AddAsync(newGenre);
        var result = await context.SaveChangesAsync();
        return result > 0
            ? new Response<string>("Genre created successfully")
            : new Response<string>("Genre not created", HttpStatusCode.InternalServerError);
    }

    public async Task<Response<string>> UpdateAsync(int id, UpdateGenreDTO genre)
    {
        var updateGenre = await context.Genres.FindAsync(id);
        if (updateGenre == null)
            return new Response<string>("Genre not found", HttpStatusCode.NotFound);

        updateGenre.Name = genre.Name;

        var result = await context.SaveChangesAsync();
        return result > 0
            ? new Response<string>("Genre not updated", HttpStatusCode.NotFound)
            : new Response<string>(null, "Genre updated");
    }

    public async Task<Response<string>> DeleteAsync(int id)
    {
        var genre = await context.Genres.FindAsync(id);
        if (genre == null)
            return new Response<string>("Genre not found", HttpStatusCode.NotFound);

        context.Genres.Remove(genre);
        var result = await context.SaveChangesAsync();
        return result > 0
            ? new Response<string>("Genre not deleted", HttpStatusCode.InternalServerError)
            : new Response<string>(null, "Genre deleted");
    }
}
