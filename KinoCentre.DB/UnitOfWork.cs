using KinoCentre.DB;
using KinoCentre.DB.Entities;

public class UnitOfWork : IDisposable
{
    private readonly KinoDbContext _context;

    public Repository<Movie> MoviesRepository { get; }
    public Repository<Ticket> TicketsRepository { get; }
    public Repository<Session> SessionsRepository { get; }

    public UnitOfWork(KinoDbContext context)
    {
        _context = context;
        
        MoviesRepository = new Repository<Movie>(context);
        TicketsRepository = new Repository<Ticket>(context);
        SessionsRepository = new Repository<Session>(context);
    }

    public async Task SaveAsync() => await _context.SaveChangesAsync();
    
    public void Dispose() => _context.Dispose();
}