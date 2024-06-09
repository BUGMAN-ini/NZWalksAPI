using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO_s;

namespace NZWalksAPI.Repositories
{
    public class WalksRepository : IWalksRepository
    {

        private readonly NZWalksDbContext _context;

        public WalksRepository(NZWalksDbContext context)
        {
            _context = context;
        }


        public async Task<Walk> CreateRegion([FromBody] Walk walk)
        {
            if (walk == null) return null;
            await _context.Walks.AddAsync(walk);
            await _context.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk?> Delete([FromRoute] Guid id)
        {
            return await _context.Walks.FirstOrDefaultAsync(x => x.Id == id);
        }


        public async Task<List<Walk>> GetAll(string? filteron = null, string? filterquery = null, string? sortBy = null, bool isascending = true, int pagenumber = 1, int pagesize = 100)
        {
            var walks = _context.Walks.Include("Difficulty").Include("Region").AsQueryable();

            if(string.IsNullOrWhiteSpace(filteron) == false && string.IsNullOrWhiteSpace(filterquery) == false )
            {
                if(filteron.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterquery));
                }
            }

            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if(sortBy.Contains("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isascending ? walks.OrderBy(x => x.Name) :  walks.OrderByDescending(x => x.Name);
                }
            }

            var skipresult = (pagenumber - 1) * pagesize;

                

            return await walks.Skip(skipresult).Take(pagesize).ToListAsync();

        }

        public async Task<Walk> GetById([FromRoute] Guid id)
        {
            var result = await _context.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (result == null) return null;
            return result;
        }

        public async Task<Walk> Update(Guid id, Walk walk)
        {
           var existingwalk = await _context.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingwalk == null) return null;

            existingwalk.Name = walk.Name;
            existingwalk.Description = walk.Description;
            existingwalk.DifficultyId = walk.DifficultyId;
            existingwalk.RegionId = walk.RegionId;
            existingwalk.LengthInKm = walk.LengthInKm;
            existingwalk.WalkImageUrl = walk.WalkImageUrl;

            await _context.SaveChangesAsync();

            return existingwalk;

        }
    }
}
