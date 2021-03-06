﻿namespace ART.Domotica.Repository.Entities
{
    using System;

    using ART.Domotica.Enums;
    using ART.Infra.CrossCutting.Repository;

    public class DeviceWiFi : IEntity<Guid>
    {
        #region Properties

        public DeviceBase DeviceBase
        {
            get; set;
        }

        public Guid DeviceDatasheetId
        {
            get; set;
        }

        public DeviceTypeEnum DeviceTypeId
        {
            get; set;
        }

        public string HostName
        {
            get; set;
        }

        public Guid Id
        {
            get; set;
        }

        public long PublishIntervalInMilliSeconds
        {
            get; set;
        }

        public string SoftAPMacAddress
        {
            get; set;
        }

        public string StationMacAddress
        {
            get; set;
        }

        #endregion Properties
    }
}