﻿namespace ART.Domotica.Domain.Services
{
    using ART.Domotica.Domain.Interfaces;
    using ART.Domotica.Enums;
    using ART.Domotica.Repository;
    using ART.Domotica.Repository.Entities;
    using ART.Domotica.Repository.Interfaces;
    using ART.Domotica.Repository.Repositories;
    using ART.Infra.CrossCutting.Domain;
    using Autofac;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class SensorInDeviceDomain : DomainBase, ISensorInDeviceDomain
    {
        #region Fields

        private readonly ISensorInDeviceRepository _sensorInDeviceRepository;
        private readonly IDeviceInApplicationRepository _deviceInApplicationRepository;

        #endregion Fields

        #region Constructors

        public SensorInDeviceDomain(IComponentContext componentContext)
        {
            var context = componentContext.Resolve<ARTDbContext>();

            _sensorInDeviceRepository = new SensorInDeviceRepository(context);
            _deviceInApplicationRepository = new DeviceInApplicationRepository(context);
        }

        #endregion Constructors

        public async Task<SensorInDevice> SetOrdination(Guid deviceSensorsId, DeviceDatasheetEnum deviceDatasheetId, Guid sensorId, SensorDatasheetEnum sensorDatasheetId, SensorTypeEnum sensorTypeId, short ordination)
        {
            var entities = await _sensorInDeviceRepository.GetByDeviceSensorsKey(deviceSensorsId, deviceDatasheetId);

            var sensorInDevice = entities
                .Where(x => x.DeviceSensorsId == deviceSensorsId)
                .Where(x => x.SensorId == sensorId)
                .Where(x => x.SensorDatasheetId == sensorDatasheetId)
                .Where(x => x.SensorTypeId == sensorTypeId)
                .FirstOrDefault();

            if (sensorInDevice == null)
            {
                throw new Exception("SensorInDevice not found");
            }

            var orderedExceptCurrent = entities
                .Except(new List<SensorInDevice> { sensorInDevice })
                .OrderBy(x => x.Ordination)
                .ToList();

            short counter = 0;

            for (short i = 0; i < orderedExceptCurrent.Count(); i++)
            {
                if (i == ordination)
                {
                    sensorInDevice.Ordination = counter;
                    counter++;
                    orderedExceptCurrent[i].Ordination = counter;
                }
                else
                {
                    orderedExceptCurrent[i].Ordination = counter;
                }
                counter++;
            }

            if(ordination == orderedExceptCurrent.Count)
            {
                sensorInDevice.Ordination = counter;
            }

            await _sensorInDeviceRepository.Update(entities);

            return sensorInDevice;
        }

        public async Task<List<SensorInDevice>> GetAllByDeviceInApplicationId(Guid applicationId, Guid deviceId, DeviceDatasheetEnum deviceDatasheetId)
        {
            var deviceInApplication = await _deviceInApplicationRepository.GetByKey(applicationId, deviceId, deviceDatasheetId);

            if (deviceInApplication == null)
            {
                throw new Exception("DeviceInApplication not found");
            }

            return await _sensorInDeviceRepository.GetAllByDeviceId(deviceInApplication.DeviceId);
        }
    }
}