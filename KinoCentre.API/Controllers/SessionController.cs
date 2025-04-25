using KinoCentre.DB.Entities;
using KinoCentre.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace KinoCentre.Controllers;


[ApiController]
[Route("api/sessions")]
public class SessionController : ControllerBase
{
    private readonly UnitOfWork unitOfWork;

    public SessionController(UnitOfWork unitOfWork)
    {   
        this.unitOfWork = unitOfWork;
    }
    
    [HttpGet("all")]
    public async Task<IActionResult> GetAllPaginatedSessions()
    {
        var entities = await unitOfWork
            .SessionsRepository
            .GetSessionsWithMovie();
        
        return Ok(entities);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSessionById(string id)
    {
        var session = await unitOfWork.SessionsRepository.GetByIdAsync(id);
        
        var tickets = await unitOfWork
            .TicketsRepository
            .GetFilteredAsync(e=>e.SessionId == id);
        
        session.TakenSeats = tickets.Select(e => e.Seat).ToList();
        
        return Ok(session);
    }
    
    [HttpGet("movieId/{movieId}")]
    public async Task<IActionResult> GetSessionsByMovieId(string movieId)
    {
        var sessions = await unitOfWork
            .SessionsRepository
            .GetFilteredAsync(e=>e.MovieId == movieId);
        
        return Ok(sessions);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddSession(SessionAddModel model)
    {
        var generatedId = ObjectId.GenerateNewId().ToString();

        var session = new Session()
        {
            Id = generatedId,
            SessionDateTime = model.SessionDateTime,
            MovieId = model.MovieId,
            RoomSize = model.RoomSize,
        };

        await unitOfWork.SessionsRepository.InsertAsync(session);
        await unitOfWork.SaveAsync();
        
        return Ok(session);
    }
}