using System.Data.Entity;

namespace BowTie.DAL.EF
{
    /// <summary>
    /// Context initializer.
    /// </summary>
    public class BowTieContextInitializer : DropCreateDatabaseIfModelChanges<BowTieContext>
    {
        protected override void Seed(BowTieContext db)
        {

        }
    }
}
