using Tutorial8.Models.DTOs;

namespace Tutorial8.Services;

public interface IClientsService
{
    Task<List<TripDTO>> GetTripsByClient(int id);
    Task<Boolean> DoesCLientExist(int id);
    Task<Boolean> DoesTripExist(int tripId);
    Task<Boolean> DoesClientHaveTrips(int id);
    Task<Boolean> IsTripBelowMax(int tripId);
    Task<Boolean> DoesRegistrationExist(int id, int tripId);
    Task<int> AddClient(ClientDTO client);
    Task<bool> AddRegistration(int id, int tripId);
    Task<Boolean> DeleteRegistration(int id, int tripId);
}