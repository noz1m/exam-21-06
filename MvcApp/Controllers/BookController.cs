using System.Net;
using Domain.DTOs.Book;
using Domain.ApiResponse;
using Domain.Filter;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MvcApp.Controllers;

public class BookController(IBookService bookService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(string? authorName, string? genreName, int pageNumber = 1, int pageSize = 10)
    {
        var filter = new BookFilter
        {
            AuthorName = authorName,
            GenreName = genreName,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var response = await bookService.GetAllAsync(filter);

        if (response.Data == null || response.StatusCode != (int)HttpStatusCode.OK)
            return View("Error");

        ViewBag.AuthorName = authorName;
        ViewBag.GenreName = genreName;
        ViewBag.PageNumber = pageNumber;
        ViewBag.PageSize = pageSize;
        ViewBag.TotalPages = response.TotalPages;

        return View(response.Data);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateBookDTO book)
    {
        if (!ModelState.IsValid)
            return View(book);

        var response = await bookService.CreateAsync(book);
        if (response.StatusCode != (int)HttpStatusCode.OK)
            return View("Error");

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var response = await bookService.GetByIdAsync(id);
        if (response.Data == null || response.StatusCode != (int)HttpStatusCode.OK)
            return View("Error");

        var book = new UpdateBookDTO
        {
            Id = response.Data.Id,
            Title = response.Data.Title,
            Description = response.Data.Description,
            AuthorId = response.Data.AuthorId,
            GenreId = response.Data.GenreId,
            PublishedYear = response.Data.PublishedYear
        };

        return View(book);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, UpdateBookDTO book)
    {
        if (!ModelState.IsValid)
            return View(book);

        var response = await bookService.UpdateAsync(id, book);
        if (response.StatusCode != (int)HttpStatusCode.OK)
            return View("Error");

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await bookService.DeleteAsync(id);
        if (response.StatusCode != (int)HttpStatusCode.OK)
            return View("Error");

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var response = await bookService.DeleteAsync(id);
        if (response.StatusCode != (int)HttpStatusCode.OK)
            return View("Error");

        return RedirectToAction("Index");
    }
}