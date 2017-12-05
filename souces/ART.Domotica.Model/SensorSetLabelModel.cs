﻿namespace ART.Domotica.Model
{
    using System;

    public class SensorSetLabelModel
    {
        #region Properties

        public Guid DeviceId
        {
            get; set;
        }

        public string Label
        {
            get; set;
        }

        public Guid SensorTempDSFamilyId
        {
            get; set;
        }

        #endregion Properties
    }
}