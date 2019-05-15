using System.Data.Entity;
using BowTie.DAL.Domain;
using BowTie.DAL.Interfaces;

namespace BowTie.DAL.EF
{
    public class BowTieContext : DbContext, IDataContext
    {
        static BowTieContext()
        {
            Database.SetInitializer<BowTieContext>(new BowTieContextInitializer());
        }

        public BowTieContext(string connectionString)
            : base(connectionString)
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }

        public IDbSet<Article> Articles { get; set; }
        public IDbSet<Event> Events { get; set; }
        public IDbSet<Region> Regions { get; set; }
        public IDbSet<EventType> EventTypes { get; set; }
        public IDbSet<City> Cities { get; set; }
        public IDbSet<District> Districts { get; set; }
        public IDbSet<Place> Places { get; set; }
        public IDbSet<Role> Roles { get; set; }
        public IDbSet<User> Users { get; set; }
        public IDbSet<DiagramUpdate> DiagramUpdates { get; set; }
        public IDbSet<SavedDiagram> SavedDiagrams { get; set; }
    }
}
