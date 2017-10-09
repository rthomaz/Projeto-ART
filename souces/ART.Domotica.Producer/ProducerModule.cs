﻿namespace ART.Domotica.Producer
{
    using ART.Domotica.Producer.Interfaces;
    using ART.Domotica.Producer.Services;

    using Autofac;

    public class ProducerModule : Module
    {
        #region Methods

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApplicationProducer>().As<IApplicationProducer>();
            builder.RegisterType<ApplicationUserProducer>().As<IApplicationUserProducer>();
            builder.RegisterType<DashboardProducer>().As<IDashboardProducer>();
            builder.RegisterType<DSFamilyTempSensorProducer>().As<IDSFamilyTempSensorProducer>();
            builder.RegisterType<HardwareProducer>().As<IHardwareProducer>();
            builder.RegisterType<HardwaresInApplicationProducer>().As<IHardwaresInApplicationProducer>();
            builder.RegisterType<TemperatureScaleProducer>().As<ITemperatureScaleProducer>();
            builder.RegisterType<ThermometerDeviceProducer>().As<IThermometerDeviceProducer>();
        }

        #endregion Methods
    }
}