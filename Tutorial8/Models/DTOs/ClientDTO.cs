namespace Tutorial8.Models.DTOs;

public class ClientDTO
{
    public int Id { get; set; }
    public List<TripDTO> Trips { get; set; } = new List<TripDTO>();
}