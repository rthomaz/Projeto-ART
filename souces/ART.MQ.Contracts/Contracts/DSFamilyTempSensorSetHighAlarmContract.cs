﻿namespace ART.MQ.Common.Contracts
{
    using System;

    [Serializable]
    public class DSFamilyTempSensorSetHighAlarmContract
    {
        #region Properties

        public Guid DSFamilyTempSensorId
        {
            get; set;
        }

        public decimal HighAlarm
        {
            get; set;
        }

        #endregion Properties
    }
}