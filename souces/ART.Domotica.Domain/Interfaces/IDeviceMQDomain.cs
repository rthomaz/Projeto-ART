﻿namespace ART.Domotica.Domain.Interfaces
{
    using System;
    using System.Threading.Tasks;

    using ART.Domotica.Repository.Entities;

    public interface IDeviceMQDomain
    {
        #region Methods

        Task<DeviceMQ> GetById(Guid deviceMQId);

        #endregion Methods
    }
}