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
    public class RegionService : IDisposable, IRegionService
    {
        private readonly IUnitOfWork db;

        public RegionService(IUnitOfWork unitOfWork)
        {
            db = unitOfWork;
        }

        public RegionDTO GetRegion(int regionId)
        {
            var region = db.Regions.Get(regionId);
            if (region == null)
                throw new ValidationException("Region not found.");
            var regionDto = Mapper.Map<Region, RegionDTO>(region);
            regionDto.EventsCount = db.Events.CountDiagramsByRegion(regionId, null, null);
            return regionDto;
        }

        public IEnumerable<RegionDTO> GetRegions(int? startYear, int? endYear)
        {
            var regions = db.Regions.GetAll().Select(r => Mapper.Map<Region, RegionDTO>(r)).ToList();
            for (var i = 0; i < regions.Count; i++)
            {
                regions[i].EventsCount = db.Events.CountDiagramsByRegion(regions[i].Id, startYear, endYear);
            }
            return regions;
        }

        public IEnumerable<RegionDTO> GetAllRegions()
        {
            var regions = db.Regions.GetAll().Select(x => Mapper.Map<Region, RegionDTO>(x)).ToList();
            for (int i = 0; i < regions.Count; i++)
            {
                regions[i].EventsCount = db.Events.CountDiagramsByRegion(regions[i].Id, null, null);
            }
            return regions;
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
