using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BowTie.BLL.DTO;
using BowTie.DAL.Domain;

namespace BowTie.BLL.Interfaces
{
    public interface IBowTieService
    {
        IEnumerable<EventTypeDTO> GetEventTypes();
        EventTypeDTO GetEvent(int code);
        RegionDTO GetRegion(int id);
        IEnumerable<RegionDTO> GetRegions(int? startYear, int? endYear);
        IEnumerable<RegionDTO> GetRegions();
        IEnumerable<dynamic> GetStats(int regionId, int? startYear, int? endYear);
        IEnumerable<DistrictDTO> GetDistricts(int regionId);
        IEnumerable<CityDTO> GetCities(int districtId);
        IEnumerable<DiagramDTO> GetDiagramsByEvent(int eventCode);
        IEnumerable<DiagramDTO> GetDiagramsByRegion(int regionId, int? startYear, int? endYear);
        IEnumerable<SaveDTO> GetSavesForDiagram(Guid diagramId);
        SaveDTO GetSave(int save);
        void DeleteSave(int id);
        Guid CreateDiagram(int eventCode, int regionId, DateTime date, string info, string name, int? districtId, int? cityId, string adress);
        void EditDiagramInfo(Guid diagramId, int eventCode, int regionId, DateTime date, string info, string name, int? cityId, int? districtId, string adress);
        void SaveDiagram(Guid diagramId, string json, int userId, bool expertCheck, string updates);
        DiagramDTO GetDiagram(Guid id);
        void DeleteDiagram(Guid diagramId);
        void Dispose();
    }
}
