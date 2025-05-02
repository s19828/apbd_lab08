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
            if (result > 0)
            {
                exists = true;
            }
        }
        
        return exists;
    }

    public async Task<bool> DoesTripExist(int tripId)
    {
        bool exists = false;
        
        string command = "SELECT Count(1) FROM Trip WHERE IdTrip = @id";
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.AddWithValue("@id", tripId);
            await conn.OpenAsync();
            
            var result = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            if (result > 0)
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

    public async Task<bool> IsTripBelowMax(int tripId)
    {
        bool belowMax = false;
        
        string command = @"SELECT Count(1), Trip.MaxPeople FROM Client_Trip 
                            LEFT JOIN Trip ON Trip.IdTrip = Client_Trip.IdTrip
                            WHERE Trip.IdTrip = @id
                            GROUP BY Trip.MaxPeople";
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.AddWithValue("@id", tripId);
            await conn.OpenAsync();
            
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (reader.GetInt32(0) < reader.GetInt32(1))
                    {
                        belowMax = true;
                    }
                }
            }
        }
        
        return belowMax;
    }

    public async Task<bool> DoesRegistrationExist(int id, int tripId)
    {
        bool exists = false;
        
        string command = "SELECT Count(1) FROM Client_Trip WHERE IdClient = @id AND IdTrip = @tripId";
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@tripId", tripId);
            await conn.OpenAsync();
            
            var result = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            if (result > 0)
            {
                exists = true;
            }
        }
        
        return exists;
    }

    public async Task<int> AddClient(ClientDTO client)
    {
        string command = @"INSERT INTO Client ( FirstName, LastName, Email, Telephone, Pesel)
                            VALUES (@FirstName, @LastName, @Email, @Telephone, @Pesel);
                            SELECT SCOPE_IDENTITY();";
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.AddWithValue("@FirstName", client.FirstName);
            cmd.Parameters.AddWithValue("@LastName", client.LastName);
            cmd.Parameters.AddWithValue("@Email", client.Email);
            cmd.Parameters.AddWithValue("@Telephone", client.Telephone);
            cmd.Parameters.AddWithValue("@Pesel", client.Pesel);
            
            await conn.OpenAsync();
            
            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }
    }

    public async Task<bool> AddRegistration(int id, int tripId)
    {
        string command = @"INSERT INTO Client_Trip ( IdClient, IdTrip, RegisteredAt)
                            VALUES (@IdClient, @IdTrip, @RegisteredAt)";
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.AddWithValue("@IdClient", id);
            cmd.Parameters.AddWithValue("@IdTrip", tripId);
            cmd.Parameters.AddWithValue("@RegisteredAt", int.Parse(DateTime.Today.ToString("yyyyMMdd")));
            
            await conn.OpenAsync();
            
            int rowsAffected = await cmd.ExecuteNonQueryAsync();
            
            return rowsAffected > 0;
        }
    }

    public async Task<bool> DeleteRegistration(int id, int tripId)
    {
        string command = "DELETE FROM Client_Trip WHERE IdClient = @IdClient AND IdTrip = @IdTrip";
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.AddWithValue("@IdClient", id);
            cmd.Parameters.AddWithValue("@IdTrip", tripId);
            
            await conn.OpenAsync();
            
            int rowsAffected = await cmd.ExecuteNonQueryAsync();
            
            return rowsAffected > 0;
        }
    }
}