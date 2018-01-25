﻿namespace ART.Domotica.IoTContract
{
    using System;

    using ART.Domotica.Enums;

    public class SensorTriggerSetTriggerValueRequestIoTContract
    {
        #region Properties

        public Guid SensorId
        {
            get; set;
        }

        public Guid SensorTriggerId
        {
            get; set;
        }

        public decimal TriggerValue
        {
            get; set;
        }

        #endregion Properties
    }
}