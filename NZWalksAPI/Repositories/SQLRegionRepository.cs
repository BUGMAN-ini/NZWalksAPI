using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO_s;

namespace NZWalksAPI.Repositories
{
    public class SQLRegionRepository : IRegionRepository
    {

        private readonly NZWalksDbContext _dbContext;

        public SQLRegionRepository(NZWalksDbContext Context)
        {
            _dbContext = Context;
        }

        public async Task<Region> CreateRegion([FromBody] Region addRegionRequest)
        {
            await _dbContext.AddAsync(addRegionRequest);
            await _dbContext.SaveChangesAsync();
            return addRegionRequest;
        }

        public async Task<Region> DeleteById([FromBody] Guid id)
        {
            return await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Region>> GetAllAsync()
        {
            return await _dbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetById([FromRoute] Guid id)
        {
            return await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Region?> Update(Guid id, Region region)
        {
            var existingregion = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (existingregion == null) return null;

            existingregion.Code = region.Code;
            existingregion.Name = region.Name;
            existingregion.RegionImageUrl = region.RegionImageUrl;

            await _dbContext.SaveChangesAsync();

            return existingregion;
        }
    }
}
