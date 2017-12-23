﻿namespace ART.Domotica.Model
{
    using System;

    using ART.Domotica.Enums;

    public class SensorInDeviceGetModel
    {
        #region Properties

        public Guid DeviceSensorsId
        {
            get; set;
        }

        public DeviceDatasheetEnum DeviceDatasheetId
        {
            get; set;
        }

        public SensorDatasheetEnum SensorDatasheetId
        {
            get; set;
        }

        public Guid SensorId
        {
            get; set;
        }

        public SensorTypeEnum SensorTypeId
        {
            get; set;
        }

        #endregion Properties
    }
}