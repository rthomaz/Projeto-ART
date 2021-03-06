﻿namespace ART.Domotica.Producer.Interfaces
{
    using System.Threading.Tasks;

    using ART.Domotica.Contract;
    using ART.Infra.CrossCutting.MQ.Contract;

    public interface ISensorTempDSFamilyProducer
    {
        #region Methods

        Task GetAllResolutions(AuthenticatedMessageContract message);

        Task SetResolution(AuthenticatedMessageContract<SensorTempDSFamilySetResolutionRequestContract> message);

        #endregion Methods
    }
}