using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using BowTie.DAL.Domain;

namespace BowTie.DAL.EF
{
    public class BowTieContext : DbContext
    {
        public BowTieContext()
            : base("DbConnection")
        { }
        public DbSet<Diagram> Diagrams { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Save> Saves { get; set; }
    }
}
