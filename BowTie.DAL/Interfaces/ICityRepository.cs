﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BowTie.DAL.Domain;

namespace BowTie.DAL.Interfaces
{
    public interface ICityRepository : IRepository<City, int>
    {
        IEnumerable<City> GetByDistrict(int districtId);
    }
}