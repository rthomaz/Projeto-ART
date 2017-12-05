﻿namespace ART.Domotica.Repository.Entities
{
    using System;
    using System.Collections.Generic;

    using ART.Infra.CrossCutting.Repository;

    public class DeviceSensors : IEntity<Guid>
    {
        #region Properties

        public DeviceBase DeviceBase
        {
            get; set;
        }

        public Guid Id
        {
            get; set;
        }

        public int PublishIntervalInSeconds
        {
            get; set;
        }

        public ICollection<SensorsInDevice> SensorsInDevice
        {
            get; set;
        }

        #endregion Properties
    }
}