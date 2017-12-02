﻿using ART.Domotica.Enums;
using ART.Domotica.Repository.Entities.SI;
using ART.Domotica.Repository.Interfaces;
using ART.Infra.CrossCutting.Repository;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace ART.Domotica.Repository.Repositories
{
    public class UnitMeasurementTypeRepository : RepositoryBase<ARTDbContext, UnitMeasurementType, UnitMeasurementTypeEnum>, IUnitMeasurementTypeRepository
    {
        public UnitMeasurementTypeRepository(ARTDbContext context) : base(context)
        {

        }

        public async Task<List<UnitMeasurementType>> GetAll()
        {
            return await _context.UnitMeasurementType
                .ToListAsync();
        }
    }
}
