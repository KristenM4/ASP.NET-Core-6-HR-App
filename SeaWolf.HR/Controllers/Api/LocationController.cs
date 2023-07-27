﻿using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SeaWolf.HR.Models;
using SeaWolf.HR.ViewModels;

namespace SeaWolf.HR.Controllers.Api
{
    [Route("api/[controller]")]
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