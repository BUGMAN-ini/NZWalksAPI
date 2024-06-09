using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO_s;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IWalksRepository walksRepository;
        private readonly IMapper mapper;
        private readonly NZWalksDbContext _context;

        public WalksController(IWalksRepository walksRepository, IMapper mapper)
        {
            this.walksRepository = walksRepository;
            this.mapper = mapper;
        }



        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await walksRepository.GetAll();
     
            return Ok(mapper.Map<List<WalkDTO>>(result));
        }

        [HttpGet]
        [Route("id:Guid")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var result = await walksRepository.GetById(id);
            return Ok(mapper.Map<WalkDTO>(result));

        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateWalkDTO walk)
        {
            var walkmodel = mapper.Map<Walk>(walk);

            walkmodel = await walksRepository.CreateRegion(walkmodel);

            var walkdto = mapper.Map<WalkDTO>(walkmodel);

            return CreatedAtAction(nameof(Get), new { Id = walkdto.Id }, walkdto);

        }

        [HttpPut]
        [Route("id:Guid")]
        public async Task<IActionResult> UpdateWalks([FromRoute] Guid id, [FromBody] UpdateWalkssDTO walks)
        {
            var walksmodel = mapper.Map<Walk>(walks);
            walksmodel = await walksRepository.Update(id, walksmodel);
            if (walksmodel == null) return BadRequest("Not Found");

            return Ok(mapper.Map<UpdateWalkssDTO>(walksmodel));

        }

    }
}
