﻿namespace ART.Domotica.Repository.Entities
{
    using System.Collections.Generic;

    using ART.Infra.CrossCutting.Repository;

    public class SensorTempDSFamilyResolution : IEntity<byte>
    {
        #region Properties

        public byte Bits
        {
            get; set;
        }

        public decimal ConversionTime
        {
            get; set;
        }

        public byte DecimalPlaces
        {
            get; set;
        }

        public string Description
        {
            get; set;
        }

        public byte Id
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }

        public decimal Resolution
        {
            get; set;
        }

        public ICollection<SensorTempDSFamily> SensorTempDSFamilies
        {
            get; set;
        }

        #endregion Properties
    }
}