﻿namespace ART.Domotica.Worker.Consumers
{
    using ART.Domotica.Constant;
    using ART.Domotica.Contract;
    using ART.Domotica.Domain.Interfaces;
    using ART.Domotica.IoTContract;
    using ART.Domotica.Model;
    using ART.Domotica.Repository.Entities;
    using ART.Domotica.Worker.IConsumers;
    using ART.Infra.CrossCutting.Logging;
    using ART.Infra.CrossCutting.MQ;
    using ART.Infra.CrossCutting.MQ.Contract;
    using ART.Infra.CrossCutting.MQ.Worker;
    using ART.Infra.CrossCutting.Setting;
    using ART.Infra.CrossCutting.Utils;
    using Autofac;
    using global::AutoMapper;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ESPDeviceConsumer : ConsumerBase, IESPDeviceConsumer
    {
        #region Fields

        private readonly EventingBasicConsumer _getAllConsumer;
        private readonly EventingBasicConsumer _getListInApplicationConsumer;        
        private readonly EventingBasicConsumer _getByPinConsumer;
        private readonly EventingBasicConsumer _insertInApplicationConsumer;
        private readonly EventingBasicConsumer _deleteFromApplicationConsumer;
        private readonly EventingBasicConsumer _getConfigurationsRPCConsumer;

        private readonly ISettingManager _settingsManager;
        private readonly IMQSettings _mqSettings;

        private readonly IComponentContext _componentContext;

        private readonly ILogger _logger;

        #endregion Fields

        #region Constructors

        public ESPDeviceConsumer(IConnection connection, ILogger logger, IComponentContext componentContext, ISettingManager settingsManager, IMQSettings mqSettings)
            : base(connection)
        {
            _getAllConsumer = new EventingBasicConsumer(_model);
            _getListInApplicationConsumer = new EventingBasicConsumer(_model);
            _getByPinConsumer = new EventingBasicConsumer(_model);
            _insertInApplicationConsumer = new EventingBasicConsumer(_model);
            _deleteFromApplicationConsumer = new EventingBasicConsumer(_model);
            _getConfigurationsRPCConsumer = new EventingBasicConsumer(_model);

            _componentContext = componentContext;

            _logger = logger;

            _settingsManager = settingsManager;
            _mqSettings = mqSettings;

            Initialize();            
        }

        #endregion Constructors

        #region Methods

        private void Initialize()
        {
            _model.QueueDeclare(
                 queue: ESPDeviceConstants.GetAllQueueName
               , durable: false
               , exclusive: false
               , autoDelete: true
               , arguments: null);

            _model.QueueDeclare(
                 queue: ESPDeviceConstants.GetListInApplicationQueueName
               , durable: false
               , exclusive: false
               , autoDelete: true
               , arguments: null);

            _model.QueueDeclare(
                 queue: ESPDeviceConstants.GetByPinQueueName
               , durable: false
               , exclusive: false
               , autoDelete: true
               , arguments: null);

            _model.QueueDeclare(
                 queue: ESPDeviceConstants.InsertInApplicationQueueName
               , durable: false
               , exclusive: false
               , autoDelete: true
               , arguments: null);

            _model.QueueDeclare(
                 queue: ESPDeviceConstants.DeleteFromApplicationQueueName
               , durable: false
               , exclusive: false
               , autoDelete: true
               , arguments: null);

            _model.QueueDeclare(
                 queue: ESPDeviceConstants.GetConfigurationsRPCQueueName
               , durable: false
               , exclusive: false
               , autoDelete: false
               , arguments: null);

            _model.QueueDeclare(
                  queue: ESPDeviceConstants.UpdatePinIoTQueueName
                , durable: true
                , exclusive: false
                , autoDelete: false
                , arguments: null);

            _model.ExchangeDeclare(
                  exchange: "amq.topic"
                , type: ExchangeType.Topic
                , durable: true
                , autoDelete: false
                , arguments: null);

            _getAllConsumer.Received += GetAllReceived;
            _getListInApplicationConsumer.Received += GetListInApplicationReceived;
            _getByPinConsumer.Received += GetByPinReceived;
            _insertInApplicationConsumer.Received += InsertInApplicationReceived;
            _deleteFromApplicationConsumer.Received += DeleteFromApplicationReceived;
            _getConfigurationsRPCConsumer.Received += GetConfigurationsRPCReceived;

            _model.BasicConsume(ESPDeviceConstants.GetAllQueueName, false, _getAllConsumer);
            _model.BasicConsume(ESPDeviceConstants.GetListInApplicationQueueName, false, _getListInApplicationConsumer);
            _model.BasicConsume(ESPDeviceConstants.GetByPinQueueName, false, _getByPinConsumer);
            _model.BasicConsume(ESPDeviceConstants.InsertInApplicationQueueName, false, _insertInApplicationConsumer);
            _model.BasicConsume(ESPDeviceConstants.DeleteFromApplicationQueueName, false, _deleteFromApplicationConsumer);
            _model.BasicConsume(ESPDeviceConstants.GetConfigurationsRPCQueueName, false, _getConfigurationsRPCConsumer);
        }

        #endregion Methods

        #region private voids

        public void GetAllReceived(object sender, BasicDeliverEventArgs e)
        {
            Task.WaitAll(GetAllReceivedAsync(sender, e));
        }

        public async Task GetAllReceivedAsync(object sender, BasicDeliverEventArgs e)
        {
            _logger.DebugEnter();

            _model.BasicAck(e.DeliveryTag, false);
            var message = SerializationHelpers.DeserializeJsonBufferToType<AuthenticatedMessageContract>(e.Body);
            var domain = _componentContext.Resolve<IESPDeviceDomain>();
            var data = await domain.GetAll();

            //Enviando para View
            var viewModel = Mapper.Map<List<ESPDevice>, List<ESPDeviceAdminDetailModel>>(data);
            var viewBuffer = SerializationHelpers.SerializeToJsonBufferAsync(viewModel);
            var exchange = "amq.topic";
            var rountingKey = string.Format("{0}-{1}", message.SouceMQSession, ESPDeviceConstants.GetAllViewCompletedQueueName);
            _model.BasicPublish(exchange, rountingKey, null, viewBuffer);

            _logger.DebugLeave();
        }

        public void GetListInApplicationReceived(object sender, BasicDeliverEventArgs e)
        {
            Task.WaitAll(GetListInApplicationReceivedAsync(sender, e));
        }
        public async Task GetListInApplicationReceivedAsync(object sender, BasicDeliverEventArgs e)
        {
            _logger.DebugEnter();

            _model.BasicAck(e.DeliveryTag, false);
            var message = SerializationHelpers.DeserializeJsonBufferToType<AuthenticatedMessageContract>(e.Body);
            var domain = _componentContext.Resolve<IESPDeviceDomain>();
            var data = await domain.GetListInApplication(message);

            //Enviando para View
            var viewModel = Mapper.Map<List<ESPDevice>, List<ESPDeviceDetailModel>>(data);
            var viewBuffer = SerializationHelpers.SerializeToJsonBufferAsync(viewModel);
            var exchange = "amq.topic";
            var rountingKey = string.Format("{0}-{1}", message.SouceMQSession, ESPDeviceConstants.GetListInApplicationViewCompletedQueueName);
            _model.BasicPublish(exchange, rountingKey, null, viewBuffer);

            _logger.DebugLeave();
        }

        public void GetByPinReceived(object sender, BasicDeliverEventArgs e)
        {
            Task.WaitAll(GetByPinReceivedAsync(sender, e));
        }

        public async Task GetByPinReceivedAsync(object sender, BasicDeliverEventArgs e)
        {
            _logger.DebugEnter();

            _model.BasicAck(e.DeliveryTag, false);
            var message = SerializationHelpers.DeserializeJsonBufferToType<AuthenticatedMessageContract<ESPDeviceGetByPinRequestContract>>(e.Body);
            var domain = _componentContext.Resolve<IESPDeviceDomain>();
            var data = await domain.GetByPin(message);

            //Enviando para View
            var exchange = "amq.topic";
            var rountingKey = string.Format("{0}-{1}", message.SouceMQSession, ESPDeviceConstants.GetByPinViewCompletedQueueName);
            var viewModel = Mapper.Map<ESPDevice, ESPDeviceGetByPinModel>(data);
            var buffer = SerializationHelpers.SerializeToJsonBufferAsync(viewModel);            
            _model.BasicPublish(exchange, rountingKey, null, buffer);

            _logger.DebugLeave();
        }

        public void InsertInApplicationReceived(object sender, BasicDeliverEventArgs e)
        {
            Task.WaitAll(InsertInApplicationReceivedAsync(sender, e));
        }

        public async Task InsertInApplicationReceivedAsync(object sender, BasicDeliverEventArgs e)
        {
            _logger.DebugEnter();

            _model.BasicAck(e.DeliveryTag, false);
            var message = SerializationHelpers.DeserializeJsonBufferToType<AuthenticatedMessageContract<ESPDeviceInsertInApplicationRequestContract>>(e.Body);
            var domain = _componentContext.Resolve<IESPDeviceDomain>();
            var data = await domain.InsertInApplication(message);

            //Enviando para View
            var exchange = "amq.topic";
            var rountingKey = string.Format("{0}-{1}", message.SouceMQSession, ESPDeviceConstants.InsertInApplicationViewCompletedQueueName);
            var viewModel = Mapper.Map<ESPDevice, ESPDeviceDetailModel>(data);
            var viewBuffer = SerializationHelpers.SerializeToJsonBufferAsync(viewModel);
            _model.BasicPublish(exchange, rountingKey, null, viewBuffer);

            //Enviando para o Iot
            var iotContract = Mapper.Map<ESPDevice, ESPDeviceInsertInApplicationResponseIoTContract>(data);
            var deviceMessage = new MessageIoTContract<ESPDeviceInsertInApplicationResponseIoTContract>(ESPDeviceConstants.InsertInApplicationIoTQueueName, iotContract);
            var deviceBuffer = SerializationHelpers.SerializeToJsonBufferAsync(deviceMessage);
            var queueName = GetDeviceQueueName(data.Id);
            _model.BasicPublish("", queueName, null, deviceBuffer);

            _logger.DebugLeave();
        }

        public void DeleteFromApplicationReceived(object sender, BasicDeliverEventArgs e)
        {
            Task.WaitAll(DeleteFromApplicationReceivedAsync(sender, e));
        }

        public async Task DeleteFromApplicationReceivedAsync(object sender, BasicDeliverEventArgs e)
        {
            _logger.DebugEnter();

            _model.BasicAck(e.DeliveryTag, false);
            var message = SerializationHelpers.DeserializeJsonBufferToType<AuthenticatedMessageContract<ESPDeviceDeleteFromApplicationRequestContract>>(e.Body);
            var domain = _componentContext.Resolve<IESPDeviceDomain>();
            var data = await domain.DeleteFromApplication(message);

            //Enviando para View
            var exchange = "amq.topic";
            var rountingKey = string.Format("{0}-{1}", message.SouceMQSession, ESPDeviceConstants.DeleteFromApplicationViewCompletedQueueName);
            var viewModel = new ESPDeviceDeleteFromApplicationModel
            {
                HardwareId = data.Id,
                HardwareInApplicationId = message.Contract.HardwareInApplicationId
            };
            var viewBuffer = SerializationHelpers.SerializeToJsonBufferAsync(viewModel);                        
            _model.BasicPublish(exchange, rountingKey, null, viewBuffer);

            //Enviando para o IoT
            var deviceMessage = new MessageIoTContract<string>(ESPDeviceConstants.DeleteFromApplicationIoTQueueName, string.Empty);
            var deviceBuffer = SerializationHelpers.SerializeToJsonBufferAsync(deviceMessage);
            var queueName = GetDeviceQueueName(data.Id);
            _model.BasicPublish("", queueName, null, deviceBuffer);

            _logger.DebugLeave();
        }

        public void GetConfigurationsRPCReceived(object sender, BasicDeliverEventArgs e)
        {
            Task.WaitAll(GetConfigurationsRPCReceivedAsync(sender, e));
        }

        public async Task GetConfigurationsRPCReceivedAsync(object sender, BasicDeliverEventArgs e)
        {
            _logger.DebugEnter();

            var requestContract = SerializationHelpers.DeserializeJsonBufferToType<ESPDeviceGetConfigurationsRPCRequestContract>(e.Body);
            var domain = _componentContext.Resolve<IESPDeviceDomain>();
            var data = await domain.GetConfigurations(requestContract);            
            
            var ntpHost = await _settingsManager.GetValueAsync<string>(SettingsConstants.NTPHostSettingsKey);
            var ntpPort = await _settingsManager.GetValueAsync<int>(SettingsConstants.NTPPortSettingsKey);
            var ntpUpdateInterval = await _settingsManager.GetValueAsync<int>(SettingsConstants.NTPUpdateIntervalSettingsKey);

            var publishMessageInterval = await _settingsManager.GetValueAsync<int>(SettingsConstants.PublishMessageIntervalSettingsKey);

            var responseContract = new ESPDeviceGetConfigurationsRPCResponseContract
            {
                BrokerHost = _mqSettings.BrokerHost,
                BrokerPort = _mqSettings.BrokerPort,
                BrokerUser = _mqSettings.BrokerUser,
                BrokerPassword = _mqSettings.BrokerPwd,
                NTPHost = ntpHost,
                NTPPort = ntpPort,
                NTPUpdateInterval = ntpUpdateInterval,
                PublishMessageInterval = publishMessageInterval,
            };

            Mapper.Map(data, responseContract);

            //Enviando para o Producer

            var buffer = SerializationHelpers.SerializeToJsonBufferAsync(responseContract);

            _model.BasicQos(0, 1, false);

            var props = e.BasicProperties;

            var replyProps = _model.CreateBasicProperties();

            replyProps.CorrelationId = props.CorrelationId;

            _model.BasicPublish(
                exchange: "", 
                routingKey: props.ReplyTo,
                basicProperties: replyProps, 
                body: buffer);

            _model.BasicAck(e.DeliveryTag, false);

            _logger.DebugLeave();
        }

        public void UpdatePins(DateTimeOffset nextFireTimeUtc)
        {
            Task.WaitAll(UpdatePinsAsync(nextFireTimeUtc));
        }

        private async Task UpdatePinsAsync(DateTimeOffset nextFireTimeUtc)
        {
            _logger.DebugEnter();

            var domain = _componentContext.Resolve<IESPDeviceDomain>();
            var data = await domain.UpdatePins();
            var contracts = Mapper.Map<List<ESPDevice>, List<ESPDeviceUpdatePinsResponseIoTContract>>(data);

            foreach (var contract in contracts)
            {
                //Enviando para o IoT
                var nextFireTimeInSeconds = nextFireTimeUtc.Subtract(DateTimeOffset.Now).TotalSeconds;
                contract.NextFireTimeInSeconds = nextFireTimeInSeconds;
                var queueName = GetDeviceQueueName(contract.HardwareId);
                var deviceMessage = new MessageIoTContract<ESPDeviceUpdatePinsResponseIoTContract>(ESPDeviceConstants.UpdatePinIoTQueueName, contract);
                var deviceBuffer = SerializationHelpers.SerializeToJsonBufferAsync(deviceMessage);
                _model.BasicPublish("", queueName, null, deviceBuffer);
            }

            _logger.DebugLeave();
        }

        #endregion Other
    }
}