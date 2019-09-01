using System;
using System.Collections.Generic;
using BowTie.BLL.DTO;

namespace BowTie.BLL.Interfaces
{
    public interface IDiagramUpdateService : IDisposable
    {
        void DeleteDiagramUpdate(int id);
        DiagramUpdateDTO CreateDiagramUpdate(DiagramUpdateDTO diagramUpdate);
        IEnumerable<DiagramUpdateDTO> GetUpdatesForDiagram(int savedDiagramId);
        DiagramUpdateDTO GetDiagramUpdate(int diagramUpdateId);
    }
}
