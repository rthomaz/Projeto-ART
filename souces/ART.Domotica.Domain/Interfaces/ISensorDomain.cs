﻿namespace ART.Domotica.Domain.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ART.Domotica.Enums;
    using ART.Domotica.Enums.SI;
    using ART.Domotica.Repository.Entities;

    public interface ISensorDomain
    {
        #region Methods

        Task<List<Sensor>> GetAllByApplicationId(Guid applicationId);

        Task<List<Sensor>> GetAllByDeviceInApplicationId(Guid deviceInApplicationId);

        Task<Sensor> GetByKey(Guid sensorId);

        Task<SensorsInDevice> GetDeviceFromSensor(Guid sensorId);

        Task<Sensor> SetAlarmBuzzerOn(Guid sensorId, SensorUnitMeasurementScalePositionEnum position, bool alarmBuzzerOn);

        Task<Sensor> SetAlarmCelsius(Guid sensorId, SensorUnitMeasurementScalePositionEnum position, decimal alarmCelsius);

        Task<Sensor> SetAlarmOn(Guid sensorId, SensorUnitMeasurementScalePositionEnum position, bool alarmOn);

        Task<Sensor> SetUnitMeasurement(Guid sensorId, UnitMeasurementEnum unitMeasurementId);

        #endregion Methods
    }
}