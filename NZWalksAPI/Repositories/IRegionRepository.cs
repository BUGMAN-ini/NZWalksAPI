using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO_s;

namespace NZWalksAPI.Repositories
{
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllAsync();
        Task<Region?> GetById([FromRoute] Guid id);
        Task<Region> CreateRegion([FromBody] Region addRegionRequest);
        Task<Region> DeleteById([FromBody] Guid id);
        Task<Region?> Update(Guid id, Region region);
    }
}
