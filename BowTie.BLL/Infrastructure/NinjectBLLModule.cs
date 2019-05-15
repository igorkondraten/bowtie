using BowTie.BLL.Interfaces;
using BowTie.BLL.Services;
using BowTie.DAL.Infrastructure;
using BowTie.DAL.Interfaces;
using BowTie.DAL.Repositories;
using Ninject.Modules;
using Ninject.Web.Common;

namespace BowTie.BLL.Infrastructure
{
    /// <summary>
    /// Business logic layer module for Ninject.
    /// </summary>
    public class NinjectBLLModule : NinjectModule
    {
        /// <summary>
        /// DAL Ninject module.
        /// </summary>
        private NinjectDALModule _DALModule;

        /// <summary>
        /// Creates Ninject module with connection string.
        /// </summary>
        /// <param name="connection">Connection string to DB.</param>
        public NinjectBLLModule(string connection)
        {
            _DALModule = new NinjectDALModule(connection);
        }

        /// <summary>
        /// Loads dependencies.
        /// </summary>
        public override void Load()
        {
            Kernel?.Load(new INinjectModule[] { _DALModule });
            Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope();
            Bind<IUserService>().To<UserService>().InRequestScope();
            Bind<IArticleService>().To<ArticleService>().InRequestScope();
            Bind<ICityService>().To<CityService>().InRequestScope();
            Bind<IDiagramUpdateService>().To<DiagramUpdateService>().InRequestScope();
            Bind<IDistrictService>().To<DistrictService>().InRequestScope();
            Bind<IEventService>().To<EventService>().InRequestScope();
            Bind<IEventTypeService>().To<EventTypeService>().InRequestScope();
            Bind<IRegionService>().To<RegionService>().InRequestScope();
            Bind<IStatsService>().To<StatsService>().InRequestScope();
            Bind<ISavedDiagramService>().To<SavedDiagramService>().InRequestScope();
        }
    }
}
