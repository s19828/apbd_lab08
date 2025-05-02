using Microsoft.AspNetCore.Mvc;
using Tutorial8.Models.DTOs;
using Tutorial8.Services;

namespace Tutorial8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientsService _clientsService;

        public ClientsController(IClientsService clientsService)
        {
            _clientsService = clientsService;
        }

        [HttpGet("{id}/trips")]
        public async Task<IActionResult> GetTripsByClient(int id)
        {
            if (!await _clientsService.DoesCLientExist(id) || !await _clientsService.DoesClientHaveTrips(id))
            {
                return NotFound();
            }
            var trips = await _clientsService.GetTripsByClient(id);
            return Ok(trips);
        }

        [HttpPost]
        public async Task<IActionResult> AddClient([FromBody] ClientDTO client)
        {
            if (client == null)
            {
                return BadRequest("No client data");
            }
            
            return Ok(await _clientsService.AddClient(client));
        }

        [HttpPut("{id}/trips/{tripId}")]
        public async Task<IActionResult> RegisterClient(int id, int tripId)
        {
            //todo
            return Ok();
        }

        [HttpDelete("{id}/trips/{tripId}")]
        public async Task<IActionResult> DeleteRegistration(int id, int tripId)
        {
            //todo
            return Ok();
        }
       
    }
}