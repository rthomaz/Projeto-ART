﻿namespace ART.Domotica.Domain.Services
{
    using System.Threading.Tasks;
    using ART.Domotica.Domain.Interfaces;
    using ART.Domotica.Repository.Interfaces;
    using ART.Domotica.Repository.Entities;
    using System;
    using ART.Infra.CrossCutting.Domain;
    using Autofac;
    using ART.Domotica.Repository;
    using ART.Domotica.Repository.Repositories;
    using ART.Domotica.Enums;

    public class DeviceWiFiDomain : DomainBase, IDeviceWiFiDomain
    {
        #region Fields

        private readonly IDeviceWiFiRepository _deviceWiFiRepository;

        #endregion Fields

        #region Constructors

        public DeviceWiFiDomain(IComponentContext componentContext)
        {
            var context = componentContext.Resolve<ARTDbContext>();
                        
            _deviceWiFiRepository = new DeviceWiFiRepository(context);
        }

        #endregion Constructors

        #region Methods

        public async Task<DeviceWiFi> SetHostName(Guid deviceWiFiId, DeviceDatasheetEnum deviceDatasheetId, string hostName)
        {
            var entity = await _deviceWiFiRepository.GetByKey(deviceWiFiId, deviceDatasheetId);

            if (entity == null)
            {
                throw new Exception("DeviceWiFi not found");
            }

            entity.HostName = hostName;

            await _deviceWiFiRepository.Update(entity);

            return entity;
        }

        #endregion Methods
    }
}