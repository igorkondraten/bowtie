namespace BowTie.BLL.DTO
{
    public class DistrictDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RegionId { get; set; }
        public RegionDTO Region { get; set; }
    }
}
