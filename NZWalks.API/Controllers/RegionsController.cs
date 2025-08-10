using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalkDbContext dbContext;

        public RegionsController(NZWalkDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        [HttpGet]
        public IActionResult GetAll() 
        {
            var Regions = dbContext.Regions.ToList();
            return Ok(Regions);

        }

        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetbyId([FromRoute] Guid id )
        {
            var Region = dbContext.Regions.FirstOrDefault(X => X.Id == id);
            if (Region == null )
            {
                return NotFound();
            }

            return Ok(Region);
        }




    }
}
