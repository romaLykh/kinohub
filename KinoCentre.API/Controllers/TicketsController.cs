using KinoCentre.DB.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace KinoCentre.Controllers;

[ApiController]
[Route("api/tickets")]
public class TicketsController : ControllerBase
{
    private readonly UnitOfWork unitOfWork;

    public TicketsController(UnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }
    
    [HttpGet("{sessionId}/taken")]
    public async Task<IActionResult> GetAllTicketsBySession(string sessionId)
    {
        var tickets = await unitOfWork.TicketsRepository.GetFilteredAsync(e => e.SessionId == sessionId);
        return Ok(tickets);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTicketById(string id)
    {
        var ticket = await unitOfWork.TicketsRepository.GetByIdAsync(id);
        return Ok(ticket);
    }

    [HttpGet("validate/{last4TicketIdDigits}")]
    public async Task<IActionResult> ValidateTicketById(string last4TicketIdDigits)
    {
        var tickets = await unitOfWork
            .TicketsRepository
            .GetAllAsync();

        if (tickets.Any(e => e.Id.ToString().EndsWith(last4TicketIdDigits)))
            return Ok();
        
        return NotFound();
    }   
    
    [HttpGet("validate/{last4TicketIdDigits}/{last4PhoneDigits}")]
    public async Task<IActionResult> ValidateicketByIdAndPhone
    (string last4TicketIdDigits, string last4PhoneDigits)
    {
        var tickets = await unitOfWork
            .TicketsRepository
            .GetAllAsync();

        if (tickets.Any(e =>
                e.Id.ToString().EndsWith(last4TicketIdDigits) &&
                e.Phone.EndsWith(last4PhoneDigits)))
        {
            return Ok();
        }

        return NotFound();
    }
    
    [HttpPost]
    public async Task<IActionResult> Purchase([FromBody] TicketPostDto ticketDto)
    {
        var generatedId = ObjectId.GenerateNewId().ToString();
        
        Console.WriteLine(ticketDto.SessionId);
        Console.WriteLine(ticketDto.Email);
        
        var ticket = new Ticket(generatedId, 
                        ticketDto.SeatRow,
                        ticketDto.SeatNumber,
                        ticketDto.SessionId,
                        ticketDto.OriginalPrice,
                        ticketDto.FinalPrice,
                        ticketDto.Email,
                        ticketDto.Phone);
        
        // var session = await unitOfWork.SessionsRepository.GetByIdAsync(ticket.Id);
        // session.TakenSeats.Add(ticket.Seat);
        // await unitOfWork.SessionsRepository.UpdateAsync(session);
        
        
        // await unitOfWork.TicketsRepository.InsertAsync(ticket);
        
        await unitOfWork.SaveAsync();
        
        var session = await unitOfWork.SessionsRepository.GetByIdAsync(ticket.SessionId);
        ticket.Session = session;
        
        var movie = await unitOfWork.MoviesRepository.GetByIdAsync(session.MovieId);
        ticket.Session.Movie = movie;
        
        // каждый билет должен быть отослан по почте в виде PDF
      
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Tickets", $"{ticket.Id}.pdf");
        
        PdfGenerator.GeneratePdf(filePath, ticket);
        await EmailSender.SendEmailAsync(ticket.Email, "Your ticket", "Here is your ticket", filePath);
        
        // удаляем файл после отправки
        
        return Ok(ticket);
    }
    
    
}

public class TicketPostDto
{
    public string? SessionId { get; set; }
    public int SeatRow { get; set; }
    public int SeatNumber { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public decimal OriginalPrice { get; set; }
    public decimal FinalPrice { get; set; }
}