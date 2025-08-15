using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

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
        public async Task<IActionResult> GetAll()
        {
            var regionsDomain = await dbContext.Regions.ToListAsync();

            //Map Domain Model To DTos
            var RegionsDto = new List<RegionDto>();
            foreach (var regionDomain in regionsDomain)
            {
                RegionsDto.Add(new RegionDto()
                {
                    Id = regionDomain.Id,
                    Code = regionDomain.Code,
                    Name = regionDomain.Name,

                });

            }

            return Ok(RegionsDto);


        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetbyId([FromRoute] Guid id)
        {
            //Get Region Domain Model From Database
            var RegionDomain = await dbContext.Regions.FirstOrDefaultAsync(X => X.Id == id);
            if (RegionDomain == null)
            {
                return NotFound();
            }

            //Map Domain Model To DTO
            var RegionDto = new RegionDto()
            {
                Id = RegionDomain.Id,
                Code = RegionDomain.Code,
                Name = RegionDomain.Name,
                RegionImageUrl = RegionDomain.RegionImageUrl
            };

            //Return DTO back to client
            return Ok(RegionDto);
        }



        // Post to Create New Region 

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {

            //Map Or Convert DTO to Domain Model

            var RegionDomainModel = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };
            //Use Domiane Model to Create New Region

            await dbContext.Regions.AddAsync(RegionDomainModel);
            await dbContext.SaveChangesAsync();

            //map Domain Model to back to DTO
            var RegionDto = new RegionDto
            {
                Id = RegionDomainModel.Id,
                Code = RegionDomainModel.Code,
                Name = RegionDomainModel.Name,
                RegionImageUrl = RegionDomainModel.RegionImageUrl
            };

            return CreatedAtAction(nameof(GetbyId), new { id = RegionDomainModel.Id }, RegionDto);

        }




        //Update Region 

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            //Get Region Domain Model From Database
            var regionDomain = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (regionDomain == null)
            {
                return NotFound();
            }
            //Map Request DTO to Domain Model
            regionDomain.Code = updateRegionRequestDto.Code;
            regionDomain.Name = updateRegionRequestDto.Name;
            regionDomain.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;
            //Save Changes to Database
            await dbContext.SaveChangesAsync();
            //Map Domain Model to DTO
            var regionDto = new RegionDto()
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl
            };
            return Ok(regionDto);
        }



        //Delete Region
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            //Get Region Domain Model From Database
            var regionDomain = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (regionDomain == null)
            {
                return NotFound();
            }
            //Delete Region
            dbContext.Regions.Remove(regionDomain);
            await dbContext.SaveChangesAsync();
            //Return No Content Response
            return NoContent();
        }
    }
}
