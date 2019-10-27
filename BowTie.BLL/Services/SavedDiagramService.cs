using System;
using BowTie.BLL.Exceptions;
using BowTie.BLL.Interfaces;
using BowTie.DAL.Interfaces;

namespace BowTie.BLL.Services
{
    public class SavedDiagramService : ISavedDiagramService
    {
        private readonly IUnitOfWork db;

        public SavedDiagramService(IUnitOfWork unitOfWork)
        {
            db = unitOfWork;
        }

        public void SetVerification(bool isVerified, int savedDiagramId)
        {
            var savedDiagram = db.SavedDiagrams.Get(savedDiagramId);
            if (savedDiagram == null)
                throw new ValidationException("Diagram not found.");
            savedDiagram.ExpertCheck = isVerified;
            savedDiagram.Date = DateTime.Now;
            db.SavedDiagrams.Update(savedDiagram);
            db.Save();
        }
    }
}
