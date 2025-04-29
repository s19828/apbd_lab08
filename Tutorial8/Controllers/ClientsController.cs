using Microsoft.AspNetCore.Mvc;
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
        
       
    }
}