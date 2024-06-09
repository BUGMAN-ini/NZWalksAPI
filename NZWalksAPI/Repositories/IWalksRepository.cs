using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO_s;

namespace NZWalksAPI.Repositories
{
    public interface IWalksRepository
    {
        Task<List<Walk>> GetAll();
        Task<Walk> GetById([FromRoute] Guid id);
        Task<Walk> Update(Guid id, Walk walk);
        Task<Walk> CreateRegion([FromBody] Walk walk);
        Task<Walk> Delete([FromRoute] Guid id);
    
    }
}
