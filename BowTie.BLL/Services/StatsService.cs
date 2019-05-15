using System;
using System.Collections.Generic;
using System.Linq;
using BowTie.BLL.DTO;
using BowTie.BLL.Interfaces;
using BowTie.DAL.Domain;
using BowTie.DAL.Interfaces;

namespace BowTie.BLL.Services
{
    public class StatsService : IDisposable, IStatsService
    {
        private readonly IUnitOfWork db;

        public StatsService(IUnitOfWork unitOfWork)
        {
            db = unitOfWork;
        }

        public IEnumerable<Stats> GetStats(int regionId, int? startYear, int? endYear)
        {
            var eventsByRegionYear = db.Events.Find(d => d.Place.RegionId == regionId &&
                                                    (!startYear.HasValue || d.EventDate.Year >= startYear.Value) &&
                                                    (!endYear.HasValue || d.EventDate.Year <= endYear.Value));
            var eventTypes = db.EventTypes.Find(x => (x.Code / 1000) == 10);
            foreach (var e in eventTypes)
            {
                int count = 0;
                var childEvents = GetChildren(eventTypes, e.Code).Select(ev => ev.Code).ToList();
                childEvents.Add(e.Code);
                foreach (var ev in childEvents)
                {
                    count += eventsByRegionYear.Count(d => d.EventTypeCode == ev);
                }
                yield return new Stats { Name = e.Name, Count = count };
            }

            IEnumerable<EventType> GetChildren(IEnumerable<EventType> types, int id)
            {
                return types
                    .Where(x => x.ParentCode == id)
                    .Union(types.Where(x => x.ParentCode == id)
                        .SelectMany(y => GetChildren(types, y.Code)));
            }
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
