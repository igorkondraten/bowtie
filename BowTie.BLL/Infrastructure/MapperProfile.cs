using AutoMapper;
using BowTie.BLL.DTO;
using BowTie.DAL.Domain;

namespace BowTie.BLL.Infrastructure
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Article, ArticleDTO>().MaxDepth(1).ReverseMap();
            CreateMap<City, CityDTO>().MaxDepth(1).ReverseMap();
            CreateMap<DiagramUpdate, DiagramUpdateDTO>().MaxDepth(1).ReverseMap();
            CreateMap<District, DistrictDTO>().MaxDepth(1).ReverseMap();
            CreateMap<Event, EventDTO>().MaxDepth(1).ReverseMap();
            CreateMap<EventType, EventTypeDTO>().MaxDepth(1).ReverseMap();
            CreateMap<Place, PlaceDTO>().MaxDepth(1).ReverseMap();
            CreateMap<Region, RegionDTO>().MaxDepth(1).ReverseMap();
            CreateMap<Role, RoleDTO>().MaxDepth(1).ReverseMap();
            CreateMap<SavedDiagram, SavedDiagramDTO>().MaxDepth(1).ReverseMap();
            CreateMap<User, UserDTO>().MaxDepth(1).ReverseMap();
        }
    }
}
