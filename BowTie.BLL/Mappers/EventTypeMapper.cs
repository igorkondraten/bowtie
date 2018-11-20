using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BowTie.DAL.Domain;
using BowTie.BLL.DTO;

namespace BowTie.BLL.Mappers
{
    public class EventTypeMapper
    {
        public EventTypeDTO Map(EventType entity)
        {
            if (entity == null) return null;
            return new EventTypeDTO
            {
                Code = entity.Code,
                Name = entity.Name,
                ParentCode = entity.ParentCode.HasValue ? entity.ParentCode.Value : 0,
                Diagrams = entity.Diagrams.Count()
            };
        }
    }
}
