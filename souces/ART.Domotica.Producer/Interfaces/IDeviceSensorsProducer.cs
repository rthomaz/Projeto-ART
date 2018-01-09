﻿namespace ART.Domotica.Producer.Interfaces
{
    using System.Threading.Tasks;

    using ART.Domotica.Contract;
    using ART.Infra.CrossCutting.MQ.Contract;

    public interface IDeviceSensorsProducer
    {
        #region Methods

        Task SetPublishIntervalInMilliSeconds(AuthenticatedMessageContract<DeviceSensorsSetPublishIntervalInMilliSecondsRequestContract> message);

        #endregion Methods
    }
}