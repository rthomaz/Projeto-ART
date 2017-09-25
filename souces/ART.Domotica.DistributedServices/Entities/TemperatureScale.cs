﻿using System;
using System.Collections.Generic;

namespace ART.Domotica.DistributedServices.Entities
{
    public class TemperatureScale : IEntity        
    {
        #region Primitive Properties

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        #endregion

        #region Navigation Properties

        public ICollection<DSFamilyTempSensor> DSFamilyTempSensors { get; set; }

        #endregion
    }
}