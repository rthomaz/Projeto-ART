﻿using ART.Domotica.Domain.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading.Tasks;
using ART.Infra.CrossCutting.MQ.Contract;
using ART.Infra.CrossCutting.MQ.Worker;
using ART.Infra.CrossCutting.Utils;
using System.Collections.Generic;
using Autofac;
using AutoMapper;
using ART.Infra.CrossCutting.Logging;
using ART.Domotica.Worker.IConsumers.SI;
using ART.Domotica.Constant.SI;
using ART.Domotica.Domain.Interfaces.SI;
using ART.Domotica.Repository.Entities.SI;
using ART.Domotica.Model.SI;
using ART.Infra.CrossCutting.MQ;

namespace ART.Domotica.Worker.Consumers.SI
{
    public class NumericalScaleTypeCountryConsumer : ConsumerBase, INumericalScaleTypeCountryConsumer
    {
        #region private fields

        private readonly EventingBasicConsumer _getAllConsumer;

        private readonly IComponentContext _componentContext;

        private readonly ILogger _logger;

        #endregion

        #region constructors

        public NumericalScaleTypeCountryConsumer(IConnection connection, ILogger logger, IComponentContext componentContext, IMQSettings mqSettings)
            : base(connection, mqSettings)
        {
            _getAllConsumer = new EventingBasicConsumer(_model);

            _componentContext = componentContext;

            _logger = logger;

            Initialize();
        }

        #endregion

        #region private voids

        private void Initialize()
        {
            _model.ExchangeDeclare(
                  exchange: "amq.topic"
                , type: ExchangeType.Topic
                , durable: true
                , autoDelete: false
                , arguments: null);

            BasicQueueDeclare(NumericalScaleTypeCountryConstants.GetAllQueueName);

            _getAllConsumer.Received += GetAllReceived;

            _model.BasicConsume(NumericalScaleTypeCountryConstants.GetAllQueueName, false, _getAllConsumer);
        }

        public void GetAllReceived(object sender, BasicDeliverEventArgs e)
        {
            Task.WaitAll(GetAllReceivedAsync(sender, e));
        }

        public async Task GetAllReceivedAsync(object sender, BasicDeliverEventArgs e)
        {
            _logger.DebugEnter();

            _model.BasicAck(e.DeliveryTag, false);
            var message = SerializationHelpers.DeserializeJsonBufferToType<AuthenticatedMessageContract>(e.Body);
            var domain = _componentContext.Resolve<INumericalScaleTypeCountryDomain>();
            var data = await domain.GetAll();

            var exchange = "amq.topic";

            var applicationMQDomain = _componentContext.Resolve<IApplicationMQDomain>();
            var applicationMQ = await applicationMQDomain.GetByApplicationUserId(message);

            //Enviando para View
            var viewModel = Mapper.Map<List<NumericalScaleTypeCountry>, List<NumericalScaleTypeCountryGetModel>>(data);
            var viewBuffer = SerializationHelpers.SerializeToJsonBufferAsync(viewModel, true);            
            var rountingKey = GetInApplicationRoutingKeyForView(applicationMQ.Topic, message.WebUITopic, NumericalScaleTypeCountryConstants.GetAllCompletedQueueName);
            _model.BasicPublish(exchange, rountingKey, null, viewBuffer);

            _logger.DebugLeave();
        }

        #endregion
    }
}
