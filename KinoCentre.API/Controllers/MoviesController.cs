using KinoCentre.DB.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace KinoCentre.Controllers;

[ApiController]
[Route("api/movies")]

public class MoviesController : ControllerBase
{
    private readonly UnitOfWork unitOfWork;

    public MoviesController(UnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllPaginatedMovies()
    {
        var entities = await unitOfWork.MoviesRepository.GetAllAsync();
        return Ok(entities);
    }
    
    [HttpGet("ongoing")]
    public async Task<IActionResult> GetOngoingMovies()
    {
        var entities = await unitOfWork.MoviesRepository.GetMoviesWithSessions();
        return Ok(entities);
    }
    
    [HttpGet("upcoming")]
    public async Task<IActionResult> GetUpcomingMovies()
    {
        var entities = await unitOfWork.MoviesRepository.GetFilteredAsync(e=>e.ReleaseDate >= DateTime.Now);
        return Ok(entities);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMovieById(string id)
    {
        var movie = await unitOfWork.MoviesRepository.GetByIdAsync(id);
        return Ok(movie);
    }
    
    [HttpGet("title/{title}")]
    public async Task<IActionResult> GetMovieByTitle(string title)
    {
        var movie = await unitOfWork
            .MoviesRepository
            .GetFilteredAsync(e => e.Title.Contains(title));
        
        if (movie == null)
        {
            return NotFound();
        }
        
        return Ok(movie);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddMovie(Movie movie)
    {
        var generatedId = ObjectId.GenerateNewId().ToString();
        
        movie.Id = generatedId;
        
        await unitOfWork.MoviesRepository.InsertAsync(movie);
        await unitOfWork.SaveAsync();
        
        return Ok();
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteMovie(string id)
    {
        var entity = await unitOfWork.MoviesRepository.GetByIdAsync(id);
        
        if(entity == null)
        {
            return NotFound();
        }
        
        await unitOfWork.MoviesRepository.DeleteAsync(entity);
        await unitOfWork.SaveAsync();
        
        return Ok();
    }
    
}