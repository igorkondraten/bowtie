namespace BowTie.DAL.Domain
{
    public class District
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RegionId { get; set; }
        public virtual Region Region { get; set; }
    }
}
