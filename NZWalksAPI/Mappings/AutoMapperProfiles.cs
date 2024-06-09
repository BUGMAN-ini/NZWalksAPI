using AutoMapper;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO_s;

namespace NZWalksAPI.Mappings
{
    public class AutoMapperProfiles : Profile
    {

        public AutoMapperProfiles()
        {
            CreateMap<Region,RegionDTO>().ReverseMap();
            CreateMap<AddRegionRequestDTO,Region>().ReverseMap();
            CreateMap<UpdateRegionRequestDTO,Region>().ReverseMap();
            CreateMap<WalkDTO,Walk>().ReverseMap();
            CreateMap<CreateWalkDTO, Walk>().ReverseMap();
            CreateMap<UpdateWalkssDTO,Walk>().ReverseMap();

        }

    }
}
