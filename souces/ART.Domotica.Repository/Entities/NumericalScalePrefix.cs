﻿namespace ART.Domotica.Repository.Entities
{
    using System.Collections.Generic;

    using ART.Domotica.Enums;
    using ART.Infra.CrossCutting.Repository;

    public class NumericalScalePrefix : IEntity<NumericalScalePrefixEnum>
    {
        #region Properties

        public NumericalScalePrefixEnum Id
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }

        public string Symbol
        {
            get; set;
        }

        public ICollection<NumericalScale> NumericalScales
        {
            get; set;
        }

        #endregion Properties
    }
}