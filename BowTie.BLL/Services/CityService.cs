using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BowTie.BLL.DTO;
using BowTie.BLL.Interfaces;
using BowTie.DAL.Domain;
using BowTie.DAL.Interfaces;

namespace BowTie.BLL.Services
{
    public class CityService : IDisposable, ICityService
    {
        private readonly IUnitOfWork db;

        public CityService(IUnitOfWork unitOfWork)
        {
            db = unitOfWork;
        }

        public IEnumerable<CityDTO> GetCitiesForDistrict(int districtId)
        {
            return db.Cities.Find(x => x.DistrictId == districtId).Select(x => Mapper.Map<City, CityDTO>(x));
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
