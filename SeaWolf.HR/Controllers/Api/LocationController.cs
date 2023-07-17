﻿using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public IActionResult GetAllLocations()
        {
            try
            {
                var allLocations = _locationRepository.AllLocations;
                return Ok(allLocations);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get Location/GetAllLocations: {ex}");
                return BadRequest("Failed to get all locationss from Location api");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetLocationDetails(int id)
        {
            try
            {
                var location = _locationRepository.GetLocationById(id);
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
    }
}