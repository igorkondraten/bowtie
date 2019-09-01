using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BowTie.BLL.DTO;
using BowTie.BLL.Exceptions;
using BowTie.BLL.Interfaces;
using BowTie.DAL.Domain;
using BowTie.DAL.Interfaces;
using DiagramType = BowTie.DAL.Domain.DiagramType;

namespace BowTie.BLL.Services
{
    public class EventService : IDisposable, IEventService
    {
        private readonly IUnitOfWork db;

        public EventService(IUnitOfWork unitOfWork)
        {
            db = unitOfWork;
        }

        public IEnumerable<EventDTO> GetEventsByCode(int eventCode)
        {
            var childEventTypes = GetChildren(db.EventTypes.GetAll(), eventCode).ToList();
            childEventTypes.Add(db.EventTypes.Get(eventCode));
            var events = new List<EventDTO>();
            foreach (var e in childEventTypes)
            {
                events = events.Concat(e.Events.Select(x => Mapper.Map<Event, EventDTO>(x))).ToList();
            }
            return events.OrderByDescending(d => d.EventDate).ToList();

            IEnumerable<EventType> GetChildren(IEnumerable<EventType> types, int id)
            {
                return types
                    .Where(x => x.ParentCode == id)
                    .Union(types.Where(x => x.ParentCode == id)
                        .SelectMany(y => GetChildren(types, y.Code)));
            }
        }

        public IEnumerable<EventDTO> GetEventsByRegion(int regionId, int? startYear, int? endYear)
        {
            return db.Events.Find(d => d.Place.RegionId == regionId &&
                                       (!startYear.HasValue || d.EventDate.Year >= startYear.Value) &&
                                       (!endYear.HasValue || d.EventDate.Year <= endYear.Value))
                .Select(x => Mapper.Map<Event, EventDTO>(x));
        }

        public Guid CreateEvent(EventDTO newEvent)
        {
            if (db.EventTypes.Get(newEvent.EventTypeCode) == null)
                throw new ValidationException("Event type not found.");
            if (newEvent.Place == null)
                throw new ValidationException("Place is null.");
            if (db.Regions.Get(newEvent.Place.RegionId) == null)
                throw new ValidationException("Region not found.");
            if (!string.IsNullOrEmpty(newEvent.Info) && newEvent.Info.Length > 50000)
                throw new ValidationException("Info must be less than 50000 characters.");
            if (string.IsNullOrEmpty(newEvent.EventName) || newEvent.EventName.Length > 80)
                throw new ValidationException("Name length must be less than 80 symbols.");
            if (!string.IsNullOrEmpty(newEvent.Place.Address) && newEvent.Place.Address.Length > 100)
                throw new ValidationException("Address length must be less than 100 symbols.");
            var createdEvent = Mapper.Map<EventDTO, Event>(newEvent);
            var savedDiagramBowTie = new SavedDiagram()
            {
                Date = DateTime.Now,
                Event = createdEvent,
                ExpertCheck = false,
                DiagramType = DiagramType.BowTie
            };
            var savedDiagramFishBone = new SavedDiagram()
            {
                Date = DateTime.Now,
                Event = createdEvent,
                ExpertCheck = false,
                DiagramType = DiagramType.Fishbone
            };
            db.SavedDiagrams.Create(savedDiagramBowTie);
            db.SavedDiagrams.Create(savedDiagramFishBone);
            createdEvent.SavedDiagrams.Add(savedDiagramBowTie);
            createdEvent.SavedDiagrams.Add(savedDiagramFishBone);
            db.Events.Create(createdEvent);
            db.Save();
            return createdEvent.Id;
        }

        public void EditEvent(EventDTO newEvent)
        {
            var oldEvent = db.Events.Get(newEvent.Id);
            if (oldEvent == null)
                throw new ValidationException("Event not found.");
            Mapper.Map<EventDTO, Event>(newEvent, oldEvent);
            db.Places.Update(oldEvent.Place);
            db.Events.Update(oldEvent);
            db.Save();
        }

        public EventDTO GetEvent(Guid id)
        {
            var eventFound = db.Events.Get(id);
            if (eventFound == null)
                throw new ValidationException("Event not found.");
            return Mapper.Map<Event, EventDTO>(eventFound);
        }

        public void DeleteEvent(Guid eventId)
        {
            if (db.Events.Get(eventId) == null)
                throw new ValidationException("Event not found.");
            db.Events.Delete(eventId);
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
