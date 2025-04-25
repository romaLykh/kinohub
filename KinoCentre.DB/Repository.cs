using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Threading.Tasks;
using KinoCentre.DB;
using KinoCentre.DB.Entities;
using MongoDB.Driver;

public class Repository<T> where T : class
{
    private readonly KinoDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(KinoDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<List<T>> GetAllAsync() => await _dbSet.ToListAsync();
    public async Task<T> GetByIdAsync(string id) => await _dbSet.FindAsync(id);
    public async Task InsertAsync(T entity) { await _dbSet.AddAsync(entity); await _context.SaveChangesAsync(); }
    public async Task UpdateAsync(T entity) { _dbSet.Update(entity); await _context.SaveChangesAsync(); }
    public async Task DeleteAsync(T entity) { _dbSet.Remove(entity); await _context.SaveChangesAsync(); }
    
    public async Task<List<T>> GetFilteredAsync(Expression<Func<T, bool>> filter)
    {
        return await _dbSet.Where(filter).ToListAsync();
    }
    
    public async Task<List<SessionWithMovie>> GetSessionsWithMovie()
    {
       var sessions = _context
               .Sessions
               .AsNoTracking()
               .AsEnumerable()
               .Where(e => e.SessionDateTime > DateTime.Now)
               .Select(e => new SessionWithMovie
               {
                   Session = e,
                   Movie = _context.Movies.Where(m => m.Id == e.MovieId).IgnoreAutoIncludes().First()
               }).ToList();
   
       return sessions;
    }

public async Task<List<Movie>> GetMoviesWithSessions()
{
    var movies = await _context
        .Movies
        .Where(e => e.ReleaseDate <= DateTime.Now)
        .ToListAsync();

    foreach (var movie in movies)
    {
        movie.Sessions = await _context.Sessions
            .Where(s => s.MovieId == movie.Id)
            .ToListAsync();
    }

    return movies;
}
    
   public class SessionWithMovie
   {
       public Session Session { get; set; }
       public Movie? Movie { get; set; }
   }
}