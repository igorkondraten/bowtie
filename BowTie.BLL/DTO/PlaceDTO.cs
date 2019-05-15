namespace BowTie.BLL.DTO
{
    public class PlaceDTO
    {
        public int Id { get; set; }
        public RegionDTO Region { get; set; }
        public int RegionId { get; set; }
        public DistrictDTO District { get; set; }
        public int? DistrictId { get; set; }
        public CityDTO City { get; set; }
        public int? CityId { get; set; }
        public string Address { get; set; }
    }
}
