using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BowTie.DAL.Interfaces;
using BowTie.DAL.Repositories;
using BowTie.DAL.EF;

namespace BowTie.DAL.Repositories
{
    public class UnitOfWork : IDisposable
    {
        private BowTieContext db = new BowTieContext();
        private IDiagramRepository diagramRepository;
        private IEventTypeRepository eventTypeRepository;
        private IRegionRepository regionRepository;
        private ICityRepository cityRepository;
        private IDistrictRepository districtRepository;
        private IPlaceRepository placeRepository;
        private IRoleRepository roleRepository;
        private ISaveRepository saveRepository;
        private IUserRepository userRepository;
        public IDiagramRepository Diagrams
        {
            get
            {
                if (diagramRepository == null)
                    diagramRepository = new DiagramRepository(db);
                return diagramRepository;
            }
        }
        public IEventTypeRepository EventTypes
        {
            get
            {
                if (eventTypeRepository == null)
                    eventTypeRepository = new EventTypeRepository(db);
                return eventTypeRepository;
            }
        }
        public IRegionRepository Regions
        {
            get
            {
                if (regionRepository == null)
                    regionRepository = new RegionRepository(db);
                return regionRepository;
            }
        }
        public ICityRepository Cities
        {
            get
            {
                if (cityRepository == null)
                    cityRepository = new CityRepository(db);
                return cityRepository;
            }
        }
        public IDistrictRepository Districts
        {
            get
            {
                if (districtRepository == null)
                    districtRepository = new DistrictRepository(db);
                return districtRepository;
            }
        }
        public IPlaceRepository Places
        {
            get
            {
                if (placeRepository == null)
                    placeRepository = new PlaceRepository(db);
                return placeRepository;
            }
        }
        public IRoleRepository Roles
        {
            get
            {
                if (roleRepository == null)
                    roleRepository = new RoleRepository(db);
                return roleRepository;
            }
        }
        public ISaveRepository Saves
        {
            get
            {
                if (saveRepository == null)
                    saveRepository = new SaveRepository(db);
                return saveRepository;
            }
        }
        public IUserRepository Users
        {
            get
            {
                if (userRepository == null)
                    userRepository = new UserRepository(db);
                return userRepository;
            }
        }
        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;
        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
