using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SeaWolf.HR.Models;
using SeaWolf.HR.ViewModels;

namespace SeaWolf.HR.Controllers.Api
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<LocationController> _logger;
        private readonly IMapper _mapper;

        public LocationController(ILocationRepository locationRepository,
            IEmployeeRepository employeeRepository, 
            ILogger<LocationController> logger,
            IMapper mapper)
        {
            _locationRepository = locationRepository;
            _employeeRepository = employeeRepository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all locations in the database
        /// </summary>
        /// <returns>IActionResult</returns>
        [HttpGet]
        public IActionResult GetAllLocations()
        {
            try
            {
                var allLocations = _locationRepository.AllLocations;
                var allLocationsMapped = _mapper.Map<IEnumerable<GetAllLocationsViewModel>>(allLocations);
                return Ok(allLocationsMapped);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get Location/GetAllLocations: {ex}");
                return BadRequest("Failed to get all locationss from Location api");
            }
        }

        /// <summary>
        /// Get a location by id number
        /// </summary>
        /// <param name="id">Id of the location to get</param>
        /// <param name="includeEmployees">Boolean to include location's employee info</param>
        /// <returns>IActionResult</returns>
        [HttpGet("{id}", Name = "GetLocationDetails")]
        public IActionResult GetLocationDetails(int id, bool includeEmployees = false)
        {
            try
            {
                var location = _locationRepository.GetLocationById(id, includeEmployees);

                if (location == null)
                {
                    return NotFound();
                }

                return Ok(location);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get Location/GetLocationDetails: {ex}");
                return BadRequest("Failed to get location from Location api");
            }
        }

        /// <summary>
        /// Add a new location to the database
        /// </summary>
        /// <param name="model">An AddLocationViewModel object with all required properties</param>
        /// <returns>A Location ActionResult with the new location's details in the database</returns>
        [HttpPost]
        public ActionResult<Location> AddLocation([FromBody] AddLocationViewModel model)
        {
            try
            {
                if (model.AddressLine2 == null) model.AddressLine2 = string.Empty;

                if (ModelState.IsValid)
                {
                    var newLocation = _mapper.Map<Location>(model);

                    _locationRepository.AddLocation(newLocation);

                    if (_locationRepository.Save())
                    {
                        return CreatedAtRoute("GetLocationDetails", new { id = newLocation.LocationId }, newLocation);
                    }
                    else return BadRequest();
                }
                else return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to post Location/AddLocation: {ex}");
                return BadRequest("Failed to add new location with Location api");
            }
        }

        /// <summary>
        /// Fully update an existing location
        /// </summary>
        /// <param name="id">Id of the location to update</param>
        /// <param name="model">An UpdateLocationViewModel object with all required properties</param>
        /// <returns>IActionResult</returns>
        [HttpPut("{id}")]
        public IActionResult UpdateLocation(int id, UpdateLocationViewModel model)
        {
            try
            {
                var location = _locationRepository.GetLocationById(id);
                if (location == null) return NotFound();
                if (model.AddressLine2 == null) model.AddressLine2 = string.Empty;

                if (ModelState.IsValid)
                {
                    _mapper.Map(model, location);

                    if (_locationRepository.Save())
                    {
                        return NoContent();

                    }
                    else return BadRequest();
                }
                else return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to put Location/UpdateLocation: {ex}");
                return BadRequest("Failed to update location details with Location api");
            }

        }

        /// <summary>
        /// Partially update an existing location using JsonPatchDocument
        /// </summary>
        /// <param name="id">Id of the location to partially update</param>
        /// <param name="patchDocument">A JsonPatchDocument object which updates an UpdateLocationViewModel property</param>
        /// <returns>IActionResult</returns>
        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdateLocation(int id, JsonPatchDocument<UpdateLocationViewModel> patchDocument)
        {
            try
            {
                var locationInDB = _locationRepository.GetLocationById(id);
                if (locationInDB == null) return NotFound();

                // map Location to UpdateLocationViewModel
                var locationToPatch = _mapper.Map<UpdateLocationViewModel>(locationInDB);

                patchDocument.ApplyTo(locationToPatch, ModelState);

                if (!ModelState.IsValid) return BadRequest(ModelState);

                if (!TryValidateModel(locationToPatch)) return BadRequest(ModelState);

                // apply changes if it passes all validation checks
                if (locationToPatch.AddressLine2 == null) locationToPatch.AddressLine2 = string.Empty;

                _mapper.Map(locationToPatch, locationInDB);

                _locationRepository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to patch Location/PartiallyUpdateLocation: {ex}");
                return BadRequest("Failed to partially update location details with Location api");
            }
        }

        /// <summary>
        /// Delete a location from the database
        /// </summary>
        /// <param name="id">Id of the location to delete</param>
        /// <returns>IActionResult</returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteLocation(int id)
        {
            try
            {
                var location = _locationRepository.GetLocationById(id);
                if (location == null) return NotFound();

                var employeesForLocation = _employeeRepository.GetEmployeesForLocation(id);
                if (employeesForLocation.Count() > 0)
                {
                    return BadRequest("Locations with employees may not be deleted. Delete or reassign this location's employees to another location.");
                }

                _locationRepository.DeleteLocation(id);

                if (_locationRepository.Save())
                {
                    return NoContent();
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to delete at Location/DeleteLocation: {ex}");
                return BadRequest("Failed to delete location with Location api");
            }
        }
    }
}