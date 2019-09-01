using System;
using System.Collections.Generic;
using BowTie.BLL.DTO;

namespace BowTie.BLL.Interfaces
{
    public interface ISavedDiagramService : IDisposable
    {
        IEnumerable<SavedDiagramDTO> GetAllSavedDiagramsForEvent(Guid eventId);
        SavedDiagramDTO GetSavedDiagram(int savedDiagramId);
        int CreateSavedDiagram(SavedDiagramDTO diagram);
        void SetVerification(bool isVerified, int savedDiagramId);
    }
}
