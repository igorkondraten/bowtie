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
    public class DistrictService : IDistrictService
    {
        private readonly IUnitOfWork db;

        public DistrictService(IUnitOfWork unitOfWork)
        {
            db = unitOfWork;
        }

        public IEnumerable<DistrictDTO> GetDistrictsForRegion(int regionId)
        {
            return db.Districts.Find(x => x.RegionId == regionId).Select(x => Mapper.Map<District, DistrictDTO>(x));
        }
    }
}
