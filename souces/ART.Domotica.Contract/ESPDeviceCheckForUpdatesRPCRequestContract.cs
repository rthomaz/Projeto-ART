﻿namespace ART.Domotica.Contract
{
    public class ESPDeviceCheckForUpdatesRPCRequestContract
    {
        #region Properties

        public string STAMacAddress
        {
            get; set;
        }

        public string APMacAddress
        {
            get; set;
        }

        public long FreeSpace
        {
            get; set;
        }

        public long SketchSize
        {
            get; set;
        }

        public long ChipSize
        {
            get; set;
        }

        public string SDKVersion
        {
            get; set;
        }

        public string Mode
        {
            get; set;
        }

        #endregion Properties
    }
}