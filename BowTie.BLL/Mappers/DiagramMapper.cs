using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BowTie.DAL.Domain;
using BowTie.BLL.DTO;

namespace BowTie.BLL.Mappers
{
    public class DiagramMapper
    {
        public DiagramDTO Map(Diagram entity)
        {
            if (entity == null) return null;
            return new DiagramDTO
            {
                Id = entity.Id,
                EventDate = entity.EventDate,
                EventType = new EventTypeDTO() { Code = entity.EventType.Code, Name = entity.EventType.Name, ParentCode = entity.EventType.ParentCode.HasValue ? entity.EventType.ParentCode.Value : 0 },
                EventTypeCode = entity.EventTypeCode,
                EventName = entity.EventName,
                ExpertCheck = entity.ExpertCheck,
                Info = entity.Info,
                PlaceId = entity.PlaceId,
                Place = new PlaceMapper().Map(entity.Place)
            };
        }
    }
}
