﻿namespace ART.Domotica.Worker
{
    using ART.Domotica.Domain;
    using ART.Domotica.Repository;
    using ART.Infra.CrossCutting.MQ;
    using ART.Domotica.Worker.AutoMapper;
    using ART.Domotica.Worker.Modules;

    using Autofac;

    using global::AutoMapper;

    using Topshelf;
    using Topshelf.Autofac;

    class Program
    {
        #region Methods

        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<WorkerService>();

            builder.RegisterType<ARTDbContext>().InstancePerDependency();

            builder.RegisterModule<RepositoryModule>();
            builder.RegisterModule<DomainModule>();
            builder.RegisterModule<MQModule>();
            builder.RegisterModule<ConsumerModule>();

            Mapper.Initialize(x =>
            {
                x.AddProfile(new ApplicationUserProfile());
                x.AddProfile(new DSFamilyTempSensorProfile());
                x.AddProfile(new TemperatureScaleProfile());
            });

            IContainer container = builder.Build();

            HostFactory.Run(c =>
            {
                c.SetServiceName("ART.Domotica.Worker");
                c.SetDisplayName("ART MQ Worker");
                c.SetDescription("A ART MQ Worker.");

                c.UseAutofacContainer(container);

                c.Service<WorkerService>(s =>
                {
                    s.ConstructUsingAutofacContainer();
                    s.WhenStarted((service, control) => service.Start());
                    s.WhenStopped((service, control) => service.Stop());
                });
            });
        }

        #endregion Methods
    }
}