using KinoCentre.DB.Entities;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;

namespace KinoCentre.DB
{
    public class KinoDbContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        private static readonly MongoClient MongoClientInstance = new MongoClient("mongodb+srv://roman:roman@kino.6vtlurx.mongodb.net/?retryWrites=true&w=majority&appName=Kino");

        public static KinoDbContext Create(IMongoDatabase database) =>
            new(new DbContextOptionsBuilder<KinoDbContext>()
                .UseMongoDB(MongoClientInstance, database.DatabaseNamespace.DatabaseName)
                .Options);

        public KinoDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var database = MongoClientInstance.GetDatabase("your-database-name");
                if (database == null)
                {
                    throw new InvalidOperationException("Database is not configured properly.");
                }
                optionsBuilder.UseMongoDB(MongoClientInstance, database.DatabaseNamespace.DatabaseName);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Movie>().ToCollection("movies");
            modelBuilder.Entity<Session>().ToCollection("sessions");
            modelBuilder.Entity<Ticket>().ToCollection("tickets");
        }
    }
}