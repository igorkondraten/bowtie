namespace BowTie.BLL.DTO
{
    public class CityDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DistrictId { get; set; }
        public DistrictDTO District { get; set; }
    }
}
