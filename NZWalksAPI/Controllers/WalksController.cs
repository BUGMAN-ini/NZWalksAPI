using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO_s;
using NZWalksAPI.Repositories;
using System.Net;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IWalksRepository _walksRepository;
        private readonly IMapper _mapper;
        private readonly NZWalksDbContext _context;
        private readonly ILogger<WalksController> _logger;

        public WalksController(IWalksRepository walksRepository, IMapper mapper, NZWalksDbContext context, ILogger<WalksController> logger)
        {
            _walksRepository = walksRepository;
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }



        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] string? filteron, [FromQuery] string? filterQuery,
                                                     [FromQuery] string? SortBy,[FromQuery] bool? isascending
                                                    ,[FromQuery] int pagenumber = 1, [FromQuery] int resultsize = 100)
        {

                var result = await _walksRepository.GetAll(filteron, filterQuery,
                                                        SortBy, isascending ?? true,
                                                        pagenumber, resultsize);

            throw new Exception("This is a new EXception");


                return Ok(_mapper.Map<List<WalkDTO>>(result));

        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var result = await _walksRepository.GetById(id);
            return Ok(_mapper.Map<WalkDTO>(result));

        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateWalkDTO walk)
        {
            var walkmodel = _mapper.Map<Walk>(walk);

            walkmodel = await _walksRepository.CreateRegion(walkmodel);

            var walkdto = _mapper.Map<WalkDTO>(walkmodel);

            return CreatedAtAction(nameof(Get), new { Id = walkdto.Id }, walkdto);

        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateWalks([FromRoute] Guid id, [FromBody] UpdateWalkssDTO walks)
        {
            var walksmodel = _mapper.Map<Walk>(walks);
            walksmodel = await _walksRepository.Update(id, walksmodel);
            if (walksmodel == null) return BadRequest("Not Found");

            return Ok(_mapper.Map<UpdateWalkssDTO>(walksmodel));

        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteWalkById([FromRoute] Guid id)
        {
            var deletewalk = await _walksRepository.Delete(id);
            if (deletewalk == null) return BadRequest();

            _context.Walks.Remove(deletewalk);

            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<WalkDTO>(deletewalk));
        }

    }
}
