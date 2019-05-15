using System.Collections.Generic;
using BowTie.DAL.Domain;

namespace BowTie.BLL.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public RoleDTO Role { get; set; }
        public string Name { get; set; }
        public IEnumerable<DiagramUpdate> DiagramUpdates { get; set; }
    }
}
