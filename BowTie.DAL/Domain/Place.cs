namespace BowTie.DAL.Domain
{
    public class Place
    {
        public int Id { get; set; }
        public virtual Region Region { get; set; }
        public int RegionId { get; set; }
        public virtual District District { get; set; }
        public int? DistrictId { get; set; }
        public virtual City City { get; set; }
        public int? CityId { get; set; }
        public string Address { get; set; }
    }
}
