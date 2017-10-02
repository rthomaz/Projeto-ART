﻿using ART.MQ.Common.QueueNames;
using ART.MQ.Worker.DSFamilyTempSensor;
using Autofac;
using MassTransit;
using System.Configuration;

namespace ART.MQ.Worker.DSFamilyTempSensor
{
    public class DSFamilyTempSensorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WorkerService>();

            builder.RegisterType<DSFamilyTempSensorSetResolutionConsumer>();

            builder.Register(context =>
            {
                var busControl = Bus.Factory.CreateUsingRabbitMq(rabbit =>
                {
                    var hostName = ConfigurationManager.AppSettings["RabbitMQHostName"];
                    var virtualHostName = ConfigurationManager.AppSettings["RabbitMQVirtualHostName"];
                    var username = ConfigurationManager.AppSettings["RabbitMQUsername"];
                    var password = ConfigurationManager.AppSettings["RabbitMQPassword"];

                    var host = rabbit.Host(hostName, virtualHostName, settings =>
                    {
                        settings.Username(username);
                        settings.Password(password);
                    });

                    rabbit.ReceiveEndpoint(host, DSFamilyTempSensorQueueNames.DSFamilyTempSensorSetResolutionQueue, e =>
                    {
                        e.Consumer<DSFamilyTempSensorSetResolutionConsumer>(context);
                    });

                });

                return busControl;
            })
                .SingleInstance()
                .As<IBusControl>()
                .As<IBus>();            
        }
    }
}
