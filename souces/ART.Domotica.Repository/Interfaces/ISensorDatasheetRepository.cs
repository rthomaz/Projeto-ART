﻿namespace ART.Domotica.Repository.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ART.Domotica.Enums;
    using ART.Domotica.Repository.Entities;
    using ART.Infra.CrossCutting.Repository;

    public interface ISensorDatasheetRepository : IRepository<ARTDbContext, SensorDatasheet, SensorDatasheetEnum>
    {
        #region Methods

        Task<List<SensorDatasheet>> GetAll();

        #endregion Methods
    }
}