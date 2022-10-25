using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Park.Models;
using Park.DataTransferObjects;

namespace Park.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly ParkContext _db;

        public LocationsController(ParkContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult<List<LocationResultDto>> Get(string name)
        {
            List<LocationResultDto> result = new List<LocationResultDto>() { };
            // Initial database query
            var query = _db.Locations.Include(e => e.Animals).AsQueryable();
            // Filter for any search params
            if (name != null)
            {
                query = _db.Locations.Where(e => e.LocationName == name);
            }
            var dbResult = query.ToList();
            // converting our db result in Data Transfer Objects
            foreach (Location e in dbResult)
            {
                // Using the constructor to convert a Location into a LocationResultDto
                LocationResultDto combine = new LocationResultDto(e);
                // Adding the DTO to your result
                result.Add(combine);
            }

            // return our list of LocationResultDtos
            return result;
        }


    }
}



