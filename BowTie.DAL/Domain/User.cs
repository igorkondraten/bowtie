using System.Collections.Generic;

namespace BowTie.DAL.Domain
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<DiagramUpdate> DiagramUpdates { get; set; }
    }
}
