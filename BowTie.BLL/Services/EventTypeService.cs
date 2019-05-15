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
    public class EventTypeService : IDisposable, IEventTypeService
    {
        private readonly IUnitOfWork db;

        public EventTypeService(IUnitOfWork unitOfWork)
        {
            db = unitOfWork;
        }

        public IEnumerable<EventTypeDTO> GetAllEventTypes()
        {
            var eventTypes = db.EventTypes.GetAll().Select(c => Mapper.Map<EventType, EventTypeDTO>(c)).ToList();
            GetTreeSum(eventTypes, 0);
            return eventTypes;

            int GetTreeSum(List<EventTypeDTO> e, int parentId)
            {
                var arr = e.Where(a => a.ParentCode.Equals(parentId)).ToList();
                var sum = 0;
                for (var i = 0; i < arr.Count; i++)
                {
                    int subEventCount = e.Count(a => a.ParentCode.Equals(arr[i].Code));
                    if (subEventCount > 0)
                    {
                        int c = GetTreeSum(e, arr[i].Code);
                        arr[i].TotalEventsCount += c;
                        sum += arr[i].TotalEventsCount;
                    }
                    else
                    {
                        sum += arr[i].TotalEventsCount;
                    }
                }
                return sum;
            }
        }

        public EventTypeDTO GetEventType(int code)
        {
            var eventType = db.EventTypes.Get(code);
            if (eventType == null)
                throw new ValidationException("Event type not found.");
            return Mapper.Map<EventType, EventTypeDTO>(eventType);
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
