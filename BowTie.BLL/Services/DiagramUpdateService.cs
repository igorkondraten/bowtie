using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BowTie.BLL.DTO;
using BowTie.BLL.Exceptions;
using BowTie.BLL.Interfaces;
using BowTie.DAL.Domain;
using BowTie.DAL.Interfaces;

namespace BowTie.BLL.Services
{
    public class DiagramUpdateService : IDiagramUpdateService
    {
        private readonly IUnitOfWork db;

        public DiagramUpdateService(IUnitOfWork unitOfWork)
        {
            db = unitOfWork;
        }

        public void DeleteDiagramUpdate(int id)
        {
            var diagramUpdate = db.DiagramUpdates.Get(id);
            if (diagramUpdate == null)
                throw new ValidationException("Diagram update not found.");
            db.DiagramUpdates.Delete(id);
            db.Save();
        }

        public DiagramUpdateDTO CreateDiagramUpdate(DiagramUpdateDTO diagramUpdate)
        {
            if (!string.IsNullOrEmpty(diagramUpdate.Updates) && diagramUpdate.Updates.Length > 500)
                throw new ValidationException("Updates length must be less than 500.");
            var update = Mapper.Map<DiagramUpdateDTO, DiagramUpdate>(diagramUpdate);
            db.DiagramUpdates.Create(update);
            db.Save();
            return Mapper.Map<DiagramUpdateDTO>(db.DiagramUpdates.Get(update.Id));
        }

        public IEnumerable<DiagramUpdateDTO> GetUpdatesForDiagram(int savedDiagramId)
        {
            return db.DiagramUpdates.Find(x => x.SavedDiagramId == savedDiagramId)
                .Select(x => Mapper.Map<DiagramUpdate, DiagramUpdateDTO>(x));
        }

        public DiagramUpdateDTO GetDiagramUpdate(int diagramUpdateId)
        {
            var diagramUpdate = db.DiagramUpdates.Get(diagramUpdateId);
            if (diagramUpdate == null)
                throw new ValidationException("Diagram update not found.");
            return Mapper.Map<DiagramUpdate, DiagramUpdateDTO>(diagramUpdate);
        }
    }
}
