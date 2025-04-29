using Microsoft.Data.SqlClient;
using Tutorial8.Models.DTOs;

namespace Tutorial8.Services;

public class ClientsService : IClientsService
{
    private readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=APBD;Integrated Security=True;";


    public async Task<List<TripDTO>> GetTripsByClient(int id)
    {
        var trips = new List<TripDTO>();

        string command = @"SELECT IdCLient, Trip.IdTrip, RegisteredAt, PaymentDate, Name, Description FROM Client_Trip
                            LEFT JOIN Trip ON Trip.IdTrip = Client_Trip.IdTrip
                            WHERE IdCLient = @id";
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.AddWithValue("@id", id);
            
            await conn.OpenAsync();

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    trips.Add(new TripDTO()
                    {
                        Id = reader.GetInt32(1),
                        Name = reader.GetString(4),
                        Description = reader.GetString(5)
                    });
                }
            }
        }
        
        return trips;
    }

    public async Task<bool> DoesCLientExist(int id)
    {
        bool exists = false;
        
        string command = "SELECT Count(1) FROM Client WHERE IdCLient = @id";
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.AddWithValue("@id", id);
            await conn.OpenAsync();
            
            var result = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            if (result != 0)
            {
                exists = true;
            }
        }
        
        return exists;
    }

    public async Task<bool> DoesClientHaveTrips(int id)
    {
        bool hasTrips = false;
        
        string command = @"SELECT Count(1) FROM Client_Trip 
                            LEFT JOIN Trip ON Trip.IdTrip = Client_Trip.IdTrip
                            WHERE IdClient = @id";
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.AddWithValue("@id", id);
            await conn.OpenAsync();
            
            var result = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            if (result > 0)
            {
                hasTrips = true;
            }
        }
        
        return hasTrips;
    }

    public Task<bool> RegisterClient(int id, int tripId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteRegistration(int id, int tripId)
    {
        throw new NotImplementedException();
    }
}