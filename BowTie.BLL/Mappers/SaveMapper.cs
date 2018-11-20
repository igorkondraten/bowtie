using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BowTie.DAL.Domain;
using BowTie.BLL.DTO;


namespace BowTie.BLL.Mappers
{
    public class SaveMapper
    {
        public SaveDTO Map(Save entity)
        {
            if (entity == null) return null;
            return new SaveDTO
            {
                Id = entity.Id,
                Date = entity.Date,
                DiagramId = entity.DiagramId,
                JsonDiagram = entity.JsonDiagram,
                Updates = entity.Updates,
                UserId = entity.UserId,
                User = new UserMapper().Map(entity.User),
                Diagram = new DiagramDTO() { EventName = entity.Diagram.EventName }
            };
        }
    }
}
