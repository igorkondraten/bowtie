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
    public class SavedDiagramService : IDisposable, ISavedDiagramService
    {
        private readonly IUnitOfWork db;

        public SavedDiagramService(IUnitOfWork unitOfWork)
        {
            db = unitOfWork;
        }

        public IEnumerable<SavedDiagramDTO> GetAllSavedDiagramsForEvent(Guid eventId)
        {
            if (db.Events.Get(eventId) == null)
                throw new ValidationException("Event not found.");
            return db.SavedDiagrams.Find(x => x.EventId == eventId)
                .Select(x => Mapper.Map<SavedDiagram, SavedDiagramDTO>(x));
        }

        public SavedDiagramDTO GetSavedDiagram(int savedDiagramId)
        {
            var savedDiagram = db.SavedDiagrams.Get(savedDiagramId);
            if (savedDiagram == null)
                throw new ValidationException("Diagram not found.");
            return Mapper.Map<SavedDiagram, SavedDiagramDTO>(savedDiagram);
        }

        public int CreateDiagram(SavedDiagramDTO diagram)
        {
            if (diagram.Event == null || db.Events.Get(diagram.EventId) == null)
                throw new ValidationException("Event not found.");
            var newDiagram = Mapper.Map<SavedDiagramDTO, SavedDiagram>(diagram);
            db.SavedDiagrams.Create(newDiagram);
            db.Save();
            return newDiagram.Id;
        }

        public void SetVerification(bool isVerified, int savedDiagramId)
        {
            var savedDiagram = db.SavedDiagrams.Get(savedDiagramId);
            if (savedDiagram == null)
                throw new ValidationException("Diagram not found.");
            savedDiagram.ExpertCheck = isVerified;
            db.SavedDiagrams.Update(savedDiagram);
            db.Save();
        }

        #region IDisposable Support
        private bool _isDisposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }

                _isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
