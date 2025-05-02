using Microsoft.Data.SqlClient;
using Tutorial8.Models.DTOs;

namespace Tutorial8.Services;

public class TripsService : ITripsService
{
    private readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=APBD;Integrated Security=True;";
    
    public async Task<List<TripDTO>> GetTrips()
    {
        var trips = new Dictionary<int, TripDTO>();

        string command = @"SELECT Trip.IdTrip, Trip.Name, Description, DateFrom, DateTo, MaxPeople, Country.Name FROM Trip 
                            LEFT JOIN Country_Trip ON Trip.IdTrip = Country_Trip.IdTrip 
                            LEFT JOIN Country ON Country_Trip.IdCountry = Country.IdCountry";
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            await conn.OpenAsync();

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    int idOrdinal = reader.GetOrdinal("IdTrip");
                    
                    int idTrip = reader.GetInt32(idOrdinal);

                    if (!trips.TryGetValue(idTrip, out TripDTO newTrip))
                    {
                        newTrip = new TripDTO 
                       {
                            Id = reader.GetInt32(idOrdinal),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2),
                            DateFrom = reader.GetDateTime(3),
                            DateTo = reader.GetDateTime(4),
                            MaxPeople = reader.GetInt32(5),
                            Countries = new List<CountryDTO>()
                        };
                       
                       trips[idTrip] = newTrip;
                    }

                    if (!reader.IsDBNull(6))
                    {
                        newTrip.Countries.Add( new CountryDTO()
                        {
                            Name = reader.GetString(6)
                        });
                    }
                    
                }
            }
        }
        
        return trips.Values.ToList();
    }
}