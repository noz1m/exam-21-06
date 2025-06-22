using System.Net;
using Domain.DTOs.Author;
using Domain.ApiResponse;
using Domain.Filter;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MvcApp.Controllers;

public class AuthorController(IAuthorService authorService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(string? name, int pageNumber = 1, int pageSize = 10)
    {
        var filter = new AuthorFilter
        {
            Name = name,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var response = await authorService.GetAllAsync(filter);

        if (response.Data == null || response.StatusCode != (int)HttpStatusCode.OK)
            return View("Error");

        ViewBag.Name = name;
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
    public async Task<IActionResult> Create(CreateAuthorDTO author)
    {
        if (!ModelState.IsValid)
            return View(author);

        var response = await authorService.CreateAsync(author);
        if (response.StatusCode != (int)HttpStatusCode.OK)
            return View("Error");

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var response = await authorService.GetByIdAsync(id);
        if (response.Data == null || response.StatusCode != (int)HttpStatusCode.OK)
            return View("Error");

        var author = new UpdateAuthorDTO
        {
            Id = response.Data.Id,
            Name = response.Data.Name,
            BirthDate = response.Data.BirthDate,
            Country = response.Data.Country
        };

        return View(author);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, UpdateAuthorDTO author)
    {
        if (!ModelState.IsValid)
            return View(author);

        var response = await authorService.UpdateAsync(id, author);
        if (response.StatusCode != (int)HttpStatusCode.OK)
            return View("Error");

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await authorService.GetByIdAsync(id);
        if (response.Data == null)
            return View("Error");

        return View(response.Data);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var response = await authorService.DeleteAsync(id);
        if (response.StatusCode != (int)HttpStatusCode.OK)
            return View("Error");

        return RedirectToAction("Index");
    }
}