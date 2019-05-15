using BowTie.DAL.Interfaces;
using Ninject.Modules;
using BowTie.DAL.EF;
using Ninject.Web.Common;

namespace BowTie.DAL.Infrastructure
{
    /// <summary>
    /// Data access layer module for Ninject.
    /// </summary>
    public class NinjectDALModule : NinjectModule
    {
        /// <summary>
        /// Connection string to DB.
        /// </summary>
        private readonly string _connection;

        /// <summary>
        /// Creates Ninject module with connection string.
        /// </summary>
        /// <param name="connection">Connection string to DB.</param>
        public NinjectDALModule(string connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Loads dependencies.
        /// </summary>
        public override void Load()
        {
            Bind<IDataContext>().To<BowTieContext>()
                .InRequestScope()
                .WithConstructorArgument(_connection);
        }
    }
}
