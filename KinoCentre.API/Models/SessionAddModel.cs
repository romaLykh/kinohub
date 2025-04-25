using KinoCentre.DB.Entities;

namespace KinoCentre.Models;

public class SessionAddModel
{
    public DateTime SessionDateTime { get; set; }

    public string MovieId { get; set; }
   
    public RoomSize RoomSize { get; set; }
}