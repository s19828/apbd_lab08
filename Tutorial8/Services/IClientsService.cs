using Tutorial8.Models.DTOs;

namespace Tutorial8.Services;

public interface IClientsService
{
    Task<List<TripDTO>> GetTripsByClient(int id);
    Task<Boolean> DoesCLientExist(int id);
    Task<Boolean> DoesClientHaveTrips(int id);
}