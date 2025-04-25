using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace KinoCentre.DB.Entities;

public class Session
{
   [BsonId] 
   public string Id { get; set; }
   public DateTime SessionDateTime { get; set; }
   
   [BsonRepresentation(BsonType.ObjectId)]

   public string MovieId { get; set; }
   
   public RoomSize RoomSize { get; set; }
   
   [BsonElement("taken_seats")]
   public List<Seat> TakenSeats { get; set; }

   public virtual Movie Movie { get; set; }
}

public enum RoomSize
{
   Default,
   Small,
   Large
}

