using Microsoft.AspNetCore.Mvc;
using SeaWolf.HR.Models;

namespace SeaWolf.HR.Controllers.Api
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class SearchLocationController : ControllerBase
    {
        private readonly ILocationRepository _locationRepository;
        private readonly ILogger<SearchLocationController> _logger;

        public SearchLocationController(ILocationRepository locationRepository, ILogger<SearchLocationController> logger)
        {
            _locationRepository = locationRepository;
            _logger = logger;
        }

        /// <summary>
        /// Find locations matching a keyword
        /// </summary>
        /// <param name="values">A keyword and sorter(LocatioName, NameDesc, City, CityDesc, Phone, PhoneDesc) separated by a '$$'. Example:
            /// <example>
            /// <code>
            /// "warehouse$$NameDesc"
            /// </code>
            /// </example>
        /// </param>
        /// <returns>IActionResult</returns>
        /// <response code="200">Returns a list of matching locations</response>
        /// <response code="400">API has failed to complete the search</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult SearchLocations([FromBody] string values)
        {
            IEnumerable<Location> locations = new List<Location>();
            IEnumerable<Location> sortedLocations;

            string[] newValues = values.Split("$$");
            string searchQuery = newValues[0];
            string sorter = newValues[1];

            try
            {
                locations = _locationRepository.SearchLocations(searchQuery);

                switch (sorter)
                {
                    case "NameDesc":
                        sortedLocations = locations.OrderByDescending(e => e.LocationName);
                        break;
                    case "City":
                        sortedLocations = locations.OrderBy(e => e.City)
                            .ThenBy(e => e.LocationName);
                        break;
                    case "CityDesc":
                        sortedLocations = locations.OrderByDescending(e => e.City)
                            .ThenBy(e => e.LocationName);
                        break;
                    case "Phone":
                        sortedLocations = locations.OrderBy(e => e.Phone)
                            .ThenBy(e => e.LocationName);
                        break;
                    case "PhoneDesc":
                        sortedLocations = locations.OrderByDescending(e => e.Phone)
                            .ThenBy(e => e.LocationName);
                        break;
                    default:
                        sortedLocations = locations;
                        break;
                }

                return Ok(sortedLocations);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get SearchLocation/SearchLocations: {ex}");
                return BadRequest("Failed to get location search from SearchLocation api");
            }
        }
    }
}

