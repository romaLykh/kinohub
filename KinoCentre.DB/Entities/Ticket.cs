namespace KinoCentre.DB.Entities;
    
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    
    public class Ticket
    {
        public Ticket()
        {
        }

        public Ticket(string id, int seatRow, int seatNumber, string sessionId, decimal originalPrice,
            decimal finalPrice, string email, string? phone)
        {
            Id = id;
            Seat = new(seatRow, seatNumber);
            SessionId = sessionId;
            OriginalPrice = originalPrice;
            FinalPrice = finalPrice;
            Email = email;
            Phone = phone;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public Seat Seat { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string SessionId { get; set; }

        public decimal OriginalPrice { get; set; }
        public decimal FinalPrice { get; set; }

        public string Email { get; set; }
        public string? Phone { get; set; }

        public virtual Session? Session { get; set; }
        public override string ToString()
        {
            return $"Ticket: {Id}, \nSeat: {Seat}, " +
                   $"\nSessionId: {SessionId}, \nOriginalPrice: {OriginalPrice}, " +
                   $"\nFinalPrice: {FinalPrice}, \nEmail: {Email}, \nPhone: {Phone}";
        }
    }