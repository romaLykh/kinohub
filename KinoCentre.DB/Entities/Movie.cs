using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace KinoCentre.DB.Entities;
public class Movie
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    
    public string Title { get; set; }
    public string Plot { get; set; }
    public string Director { get; set; }
    
    public int Duration { get; set; }
    public string Language { get; set; }
    
    public DateTime ReleaseDate { get; set; }
    
    
    public string PosterUrl { get; set; }
    public string TrailerUrl { get; set; }
    public string? PromoCode { get; set; }
    public int PromoUsageLeft { get; set; }
    
    
    
    [BsonElement("spoilers_urls")]
    public List<string> SpoilersUrls { get; set; }
    [BsonElement("sessions")]
    public List<Session> Sessions { get; set; }
    [BsonElement("genres")]
    public List<string> Genres { get; set; }
    [BsonElement("actors")]
    public List<string> Actors { get; set; }
}

