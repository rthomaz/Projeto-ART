﻿namespace ART.Domotica.Contract
{
    using System;

    public class DSFamilyTempSensorSetAlarmCelsiusRequestContract
    {
        #region Properties

        public decimal AlarmCelsius
        {
            get; set;
        }

        public Guid DSFamilyTempSensorId
        {
            get; set;
        }

        public TempSensorAlarmPositionContract Position
        {
            get; set;
        }

        #endregion Properties
    }
}