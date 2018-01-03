﻿namespace ART.Domotica.Contract
{
    using System;

    using ART.Domotica.Enums;

    public class DeviceWiFiSetHostNameRequestContract
    {
        #region Properties

        public DeviceDatasheetEnum DeviceDatasheetId
        {
            get; set;
        }

        public Guid DeviceWiFiId
        {
            get; set;
        }

        public string HostName
        {
            get; set;
        }

        #endregion Properties
    }
}