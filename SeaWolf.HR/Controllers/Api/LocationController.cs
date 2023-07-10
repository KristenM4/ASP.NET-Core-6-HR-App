using Microsoft.AspNetCore.Mvc;
using SeaWolf.HR.Models;

namespace SeaWolf.HR.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationRepository _locationRepository;
        private readonly ILogger<LocationController> _logger;

        public LocationController(ILocationRepository locationRepository, ILogger<LocationController> logger)
        {
            _locationRepository = locationRepository;
            _logger = logger;
        }
    }
}