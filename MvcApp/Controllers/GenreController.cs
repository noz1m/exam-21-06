using System.Net;
using Domain.DTOs.Genre;
using Domain.ApiResponse;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MvcApp.Controllers;

public class GenreController(IGenreService genreService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Idex()
    {
        var response = await genreService.GetAllAsync();
        if (response.Data == null || !response.IsSuccess.Equals(HttpStatusCode.OK))
            return View("Error");

        return View(response.Data);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateGenreDTO genre)
    {
        if (!ModelState.IsValid)
            return View(genre);

        var response = await genreService.CreateAsync(genre);
        if (!response.IsSuccess.Equals(HttpStatusCode.OK))
            return View("Error");
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var response = await genreService.GetByIdAsync(id);
        if (response.Data == null || !response.StatusCode.Equals(HttpStatusCode.OK))
            return View("Error");

        var genre = new UpdateGenreDTO
        {
            Id = response.Data.Id,
            Name = response.Data.Name
        };

        return View(genre);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, UpdateGenreDTO genre)
    {
        if (!ModelState.IsValid)
            return View(genre);

        var response = await genreService.UpdateAsync(id, genre);
        if (!response.IsSuccess.Equals(HttpStatusCode.OK))
            return View("Error");
        return RedirectToAction("Index");
    }

    // [HttpGet]
    // public async Task<IActionResult> Delete(int id)
    // {
    //     var response = await genreService.DeleteAsync(id);
    //     if (!response.IsSuccess.Equals(HttpStatusCode.OK))
    //         return View("Error");
    //     return RedirectToAction("Index");
    // }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await genreService.DeleteAsync(id);
        if (!response.StatusCode.Equals(HttpStatusCode.OK))
            return View("Error");

        return RedirectToAction("Index");
    }
}
