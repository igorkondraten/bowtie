namespace BowTie.DAL.Domain
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DistrictId { get; set; }
        public virtual District District { get; set; }
    }
}
