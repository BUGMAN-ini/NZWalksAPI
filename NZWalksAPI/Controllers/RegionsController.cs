using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
    
    public class RegionsController : ControllerBase
    {

        private readonly NZWalksDbContext _context;
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;

        public RegionsController(NZWalksDbContext context, IRegionRepository regionRepository, IMapper mapper)
        {
            _context = context;
            _regionRepository = regionRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var regions = await _regionRepository.GetAllAsync();
            //var regiondto = new List<RegionDTO>();
            //foreach (var r in regions)
            //{
            //    regiondto.Add(new RegionDTO()
            //    {
            //        Id = r.Id,
            //        Name = r.Name,
            //        Code = r.Code,
            //        RegionImageUrl = r.RegionImageUrl
            //    });
            //}
            return Ok(_mapper.Map<List<RegionDTO>>(regions));
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Get([FromRoute] Guid id) 
        {
            var result = await _regionRepository.GetById(id);
            
      
            if (result == null) return NotFound("Not Found Maan!");

            //var regiondto = new RegionDTO
            //{
            //    Id = result.Id,
            //    Name = result.Name,
            //    Code = result.Code,
            //    RegionImageUrl = result.RegionImageUrl
            //};

            return Ok(_mapper.Map<RegionDTO>(result));
        }

        [HttpPost]
        public async Task<IActionResult> CreateRegion([FromBody] AddRegionRequestDTO addRegionRequest)
        {
            //var regiondomainModel = new Region
            //{
            //    Code = addRegionRequest.Code,
            //    Name = addRegionRequest.Name,
            //    RegionImageUrl=addRegionRequest.RegionImageUrl
            //};

            var regiondomainmodel = _mapper.Map<Region>(addRegionRequest);


            regiondomainmodel = await _regionRepository.CreateRegion(regiondomainmodel);

            //var RegionDTO = new RegionDTO
            //{
            //    Id= result.Id,
            //    Name = result.Name,
            //    Code = result.Code,
            //    RegionImageUrl = result.RegionImageUrl
            //};

            var regiondto = _mapper.Map<RegionDTO>(regiondomainmodel);
            
            return CreatedAtAction(nameof(Get), new { Id = regiondto.Id}, regiondto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO updateregionRequestDTO)
        {
            var regionDomainModel = _mapper.Map<Region>(updateregionRequestDTO);

            regionDomainModel = await _regionRepository.Update(id, regionDomainModel);
            if (regionDomainModel == null) return BadRequest("Not Found");

            await _context.SaveChangesAsync();

            var regiondto = _mapper.Map<RegionDTO>(regionDomainModel);

            return Ok(regiondto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regiondomainmodel = await _regionRepository.DeleteById(id);
            if (regiondomainmodel == null) return BadRequest();

            _context.Regions.Remove(regiondomainmodel);

            await _context.SaveChangesAsync();

            //var regionDTO = new RegionDTO
            //{
            //    Id = regiondomainmodel.Id,
            //    Code = regiondomainmodel.Code,
            //    Name = regiondomainmodel.Name,
            //    RegionImageUrl = regiondomainmodel.RegionImageUrl
            //};

            var regiondto = _mapper.Map<RegionDTO>(regiondomainmodel);
            return Ok(regiondto);
        }
          
    }
}
