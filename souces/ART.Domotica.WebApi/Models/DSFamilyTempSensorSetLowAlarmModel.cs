﻿namespace ART.Domotica.WebApi.Models
{
    using System;

    public class DSFamilyTempSensorSetLowAlarmModel
    {
        #region Properties

        public Guid DSFamilyTempSensorId
        {
            get; set;
        }

        public decimal LowAlarm
        {
            get; set;
        }

        #endregion Properties
    }
}