using System;
using BowTie.DAL.Interfaces.Repositories;

namespace BowTie.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IArticleRepository Articles { get; }
        ICityRepository Cities { get; }
        IDiagramUpdateRepository DiagramUpdates { get; }
        IDistrictRepository Districts { get; }
        IEventRepository Events { get; }
        IPlaceRepository Places { get; }
        IRegionRepository Regions { get; }
        IRoleRepository Roles { get; }
        ISavedDiagramRepository SavedDiagrams { get; }
        IUserRepository Users { get; }
        IEventTypeRepository EventTypes { get; }
        void Save();
    }
}
