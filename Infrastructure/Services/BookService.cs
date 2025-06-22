using AutoMapper;
using System.Net;
using Domain.Entities;
using Domain.ApiResponse;
using Domain.DTOs;
using Domain.DTOs.Book;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Domain.Filter;

namespace Infrastructure.Services;

public class BookService(DataContext context) : IBookService
{
    public async Task<PagedResponse<List<GetBookDTO>>> GetAllAsync(BookFilter filter)
    {
        var validFilter = new ValidFilter(filter.PageNumber, filter.PageSize);
        var books = context.Books
            .Include(b => b.Author)
            .Include(b => b.Genre)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.AuthorName))
        {
            var trimAuthor = filter.AuthorName.Trim().ToLower();
            var author = await context.Authors
                .FirstOrDefaultAsync(a => a.Name.ToLower() == trimAuthor);

            if (author == null)
                return new PagedResponse<List<GetBookDTO>>("Author with such name not found",HttpStatusCode.NotFound);

            books = books.Where(b => b.AuthorId == author.Id);
        }

        if (!string.IsNullOrWhiteSpace(filter.GenreName))
        {
            var trimGenre = filter.GenreName.Trim().ToLower();
            var genre = await context.Genres
                .FirstOrDefaultAsync(g => g.Name.ToLower() == trimGenre);

            if (genre == null)
                return new PagedResponse<List<GetBookDTO>>("Genre with such name not found",HttpStatusCode.NotFound);

            books = books.Where(b => b.GenreId == genre.Id);
        }

        var totalCount = await books.CountAsync();

        var data = await books
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .Select(b => new GetBookDTO
            {
                Id = b.Id,
                Title = b.Title,
                Description = b.Description,
                AuthorId = b.AuthorId,
                GenreId = b.GenreId,
                PublishedYear = b.PublishedYear
            })
            .ToListAsync();

        return new PagedResponse<List<GetBookDTO>>(data,totalCount,validFilter.PageNumber,validFilter.PageSize );
    }

    public async Task<Response<GetBookDTO>> GetByIdAsync(int id)
    {
        var book = await context.Books.FindAsync(id);
        if (book == null)
            return new Response<GetBookDTO>("Book not found", HttpStatusCode.InternalServerError);

        var GetBookDTO = new GetBookDTO
        {
            Id = book.Id,
            Title = book.Title,
            Description = book.Description,
            AuthorId = book.AuthorId,
            GenreId = book.GenreId,
            PublishedYear = book.PublishedYear
        };

        return GetBookDTO == null
            ? new Response<GetBookDTO>("Book not found", HttpStatusCode.NotFound)
            : new Response<GetBookDTO>(GetBookDTO, "Book found");
    }

    public async Task<Response<string>> CreateAsync(CreateBookDTO book)
    {
        if (book == null)
            return new Response<string>("Invalid book data", HttpStatusCode.BadRequest);

        var newBook = new Book
        {
            Title = book.Title,
            Description = book.Description,
            PublishedYear = book.PublishedYear,
            AuthorId = book.AuthorId,
            GenreId = book.GenreId,
        };

        await context.Books.AddAsync(newBook);
        var result = await context.SaveChangesAsync();

        return result > 0
            ? new Response<string>("Book successfully created", HttpStatusCode.OK)
            : new Response<string>("Book not created", HttpStatusCode.InternalServerError);
    }

    public async Task<Response<string>> UpdateAsync(int id, UpdateBookDTO book)
    {
        var updateBook = await context.Books.FindAsync(id);
        if (updateBook == null)
            return new Response<string>("Book not found", HttpStatusCode.NotFound);

        updateBook.Title = book.Title;
        updateBook.Description = book.Description;
        updateBook.AuthorId = book.AuthorId;
        updateBook.GenreId = book.GenreId;
        updateBook.PublishedYear = book.PublishedYear;

        var result = await context.SaveChangesAsync();
        return result > 0
            ? new Response<string>("Book not updated", HttpStatusCode.NotFound)
            : new Response<string>(null, "Book updated");
    }

    public async Task<Response<string>> DeleteAsync(int id)
    {
        var book = await context.Books.FindAsync(id);
        if (book == null)
            return new Response<string>("Book not found", HttpStatusCode.NotFound);

        context.Books.Remove(book);
        var result = await context.SaveChangesAsync();
        return result > 0
            ? new Response<string>("Book not deleted", HttpStatusCode.InternalServerError)
            : new Response<string>(null, "Book deleted");
    }
}
