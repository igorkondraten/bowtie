using System;
using BowTie.DAL.Interfaces;
using BowTie.DAL.Interfaces.Repositories;

namespace BowTie.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly IDataContext _context;
        private IEventRepository _eventRepository;
        private IEventTypeRepository _eventTypeRepository;
        private IRegionRepository _regionRepository;
        private ICityRepository _cityRepository;
        private IDistrictRepository _districtRepository;
        private IPlaceRepository _placeRepository;
        private IRoleRepository _roleRepository;
        private ISavedDiagramRepository _savedDiagramRepository;
        private IUserRepository _userRepository;
        private IDiagramUpdateRepository _diagramUpdateRepository;
        private IArticleRepository _articleRepository;

        public IArticleRepository Articles => _articleRepository ?? (_articleRepository = new ArticleRepository(_context));
        public ICityRepository Cities => _cityRepository ?? (_cityRepository = new CityRepository(_context));
        public IDiagramUpdateRepository DiagramUpdates => _diagramUpdateRepository ?? (_diagramUpdateRepository = new DiagramUpdateRepository(_context));
        public IDistrictRepository Districts => _districtRepository ?? (_districtRepository = new DistrictRepository(_context));
        public IEventRepository Events => _eventRepository ?? (_eventRepository = new EventRepository(_context));
        public IEventTypeRepository EventTypes => _eventTypeRepository ?? (_eventTypeRepository = new EventTypeRepository(_context));
        public IPlaceRepository Places => _placeRepository ?? (_placeRepository = new PlaceRepository(_context));
        public IRegionRepository Regions => _regionRepository ?? (_regionRepository = new RegionRepository(_context));
        public IRoleRepository Roles => _roleRepository ?? (_roleRepository = new RoleRepository(_context));
        public ISavedDiagramRepository SavedDiagrams => _savedDiagramRepository ?? (_savedDiagramRepository = new SavedDiagramRepository(_context));
        public IUserRepository Users => _userRepository ?? (_userRepository = new UserRepository(_context));

        public UnitOfWork(IDataContext context)
        {
            _context = context;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        #region IDisposable Support
        private bool _isDisposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _context?.Dispose();
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
