using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace BowTie.DAL.Interfaces
{
    /// <summary>
    /// Interface for database context.
    /// </summary>
    public interface IDataContext : IDisposable
    {
        /// <summary>
        /// Returns a DbSet instance for access to entities of the given type in the context.
        /// </summary>
        /// <typeparam name="TEntity">Type of entity.</typeparam>
        /// <returns>DbSet for entity.</returns>
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        /// <summary>
        /// Saves all changes made in context to the database.
        /// </summary>
        /// <returns>The number of entries written to the database.</returns>
        int SaveChanges();

        /// <summary>
        /// Gets a DbEntityEntry object for the given entity providing access to information about the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns>An entry for the entity.</returns>
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}
