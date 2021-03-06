﻿namespace ART.Infra.CrossCutting.MQ.WebApi
{
    using System.Linq;

    using ART.Infra.CrossCutting.MQ.Contract;
    using ART.Infra.CrossCutting.WebApi;

    public abstract class AuthenticatedMQApiControllerBase : AuthenticatedApiControllerBase
    {
        #region Properties

        protected string _webUITopic
        {
            get
            {
                var value = string.Empty;
                if (Request.Headers.Contains("webUITopic"))
                {
                    var values = Request.Headers.GetValues("webUITopic");
                    value = values.SingleOrDefault();
                }
                return value;
            }
        }

        #endregion Properties

        #region Methods

        protected AuthenticatedMessageContract CreateMessage()
        {
            return new AuthenticatedMessageContract
            {
                WebUITopic = _webUITopic,
                ApplicationUserId = _applicationUserId,
            };
        }

        protected AuthenticatedMessageContract<TContract> CreateMessage<TContract>(TContract contract)
        {
            return new AuthenticatedMessageContract<TContract>
            {
                WebUITopic = _webUITopic,
                ApplicationUserId = _applicationUserId,
                Contract = contract,
            };
        }

        #endregion Methods
    }
}