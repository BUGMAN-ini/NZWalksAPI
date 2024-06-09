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
            var result = await _context.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (result == null) return null;

            return result;
        }


        public async Task<List<Walk>> GetAll()
        {
            return await _context.Walks.Include("Difficulty").Include("Region").ToListAsync();
            
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
