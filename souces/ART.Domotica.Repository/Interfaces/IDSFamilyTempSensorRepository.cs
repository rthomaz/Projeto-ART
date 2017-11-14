﻿namespace ART.Domotica.Repository.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ART.Domotica.Repository.Entities;
    using ART.Infra.CrossCutting.Repository;

    public interface IDSFamilyTempSensorRepository : IRepository<ARTDbContext, DSFamilyTempSensor, Guid>
    {
        #region Methods

        Task<List<DSFamilyTempSensor>> GetAllByDeviceId(Guid deviceId);

        Task<SensorsInDevice> GetDeviceFromSensor(Guid dsFamilyTempSensorId);

        Task<List<DSFamilyTempSensor>> GetListInApplication(Guid applicationUserId);

        #endregion Methods
    }
}