using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Park.Models;

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
        public async Task<ActionResult<IEnumerable<Location>>> Get(string name)
        {
            // Putting the .Include here will show the animals in the result. Can lead to infinite serialization issues if not ignoring 
            var query = _db.Locations.Include(e => e.Animals).AsQueryable();
            if (name != null)
            {
                query = query.Where(e => e.LocationName == name);
            }

            return await query.ToListAsync();
        }


    }
}



